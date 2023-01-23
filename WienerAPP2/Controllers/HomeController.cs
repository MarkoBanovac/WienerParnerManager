using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WienerAPP2.Models;
//using Microsoft.AspNetCore.Mvc;

namespace WienerAPP2.Controllers
{
    public class HomeController : Controller
    {
        private List<Partner> _partners = new List<Partner>();
        DbConnection db = new DbConnection("Server=127.0.0.1;Port=3306;Database=partners;Uid=root;Pwd=sqlpartners;");


        public ActionResult Index(bool fromCreate = false, string partnerCode = "")
        {
            db.Open();
            var _partnersDB = db.GetAllPartners();
            db.Close();

            _partnersDB = _partnersDB.OrderByDescending(p => p.CreatedAtUtc).ToList();

            foreach(Partner partner in _partnersDB)
            {
                int policyCount = 0;
                decimal policySum = 0;

                db.Open();
                var partnersPolicies = db.GetAllPartnerPolicies(partner.ExternalCode);
                db.Close();

                foreach(Policy policy in partnersPolicies)
                {
                    policyCount += 1;
                    policySum += policy.Value;
                }

                if(policyCount > 5 || policySum > 5000)
                {
                    partner.FirstName = "*" + partner.FirstName;
                }

                if (fromCreate == true && partnerCode == partner.ExternalCode)
                {
                    TempData["NewEntry"] = partner.ExternalCode;
                }
            }

            return View(_partnersDB);
        }

        public ActionResult NewPartner()
        {
            ViewBag.Message = "Input new partner information below.";
            return View();
        }

        public ActionResult NewPolicy()
        {
            ViewBag.Message = "Input new policy information below.";

            return View();
        }

        [HttpPost]
        public ActionResult Create(Partner partner)
        {
            db.Open();
            var _partner = db.GetOnePartner(partner.ExternalCode);
            db.Close();
            if (_partner.Count() > 0)
            {
                foreach (var key in ModelState.Keys)
                {
                    TempData[key] = ModelState[key].Value.AttemptedValue;
                }

                List<string> errorList = new List<string>();
                errorList.Add("Partner with that external code already exists");
                TempData["Errors"] = errorList;
                TempData.Keep("Errors");
                return RedirectToAction("NewPartner");
            }
            else if (TryValidateModel(partner))
            {
                partner.CreatedAtUtc = DateTime.UtcNow;
                db.Open();
                db.AddPartner(partner);
                db.Close();
                return RedirectToAction("Index", new { fromCreate = true, partnerCode = partner.ExternalCode });
            }
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    if (key != "partner.CreatedAtUtc")
                    {
                        TempData[key] = ModelState[key].Value.AttemptedValue;
                    }
                }

                List<string> errorList = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        if (!errorList.Contains(error.ErrorMessage))
                        {
                            errorList.Add(error.ErrorMessage);
                        }
                    }
                }

                TempData["Errors"] = errorList;
                TempData.Keep("Errors");
                return RedirectToAction("NewPartner");
            }
        }

        [HttpPost]
        public ActionResult CreatePolicy(Policy policy)
        {
            db.Open();
            var _partner = db.GetOnePartner(policy.ExternalCode);
            db.Close();
            if(_partner.Count() == 0)
            {
                foreach (var key in ModelState.Keys)
                {
                    TempData[key] = ModelState[key].Value.AttemptedValue;
                }

                List<string> errorList = new List<string>();
                errorList.Add("Partner not found, please try again");
                TempData["Errors"] = errorList;
                TempData.Keep("Errors");
                return RedirectToAction("NewPolicy");
            }
            else if (TryValidateModel(policy))
            {
                db.Open();
                db.AddPolicy(policy);
                db.Close();
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    TempData[key] = ModelState[key].Value.AttemptedValue;
                }

                List<string> errorList = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        if (!errorList.Contains(error.ErrorMessage))
                        {
                            errorList.Add(error.ErrorMessage);
                        }
                    }
                }

                TempData["Errors"] = errorList;
                TempData.Keep("Errors");
                return RedirectToAction("NewPolicy");
            }
        }

        public ActionResult OpenModalPartner(string ExternalCode)
        {
            db.Open();
            var _partner = db.GetOnePartner(ExternalCode);
            db.Close();

            return View(_partner);
        }
    }
}