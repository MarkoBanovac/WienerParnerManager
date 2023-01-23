using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WienerAPP2.Models
{
    public class Partner
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(maximumLength: 255, MinimumLength = 2, ErrorMessage = "First Name must be between 5 and 100 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(maximumLength: 255, MinimumLength = 2, ErrorMessage = "Last Name must be between 5 and 100 characters")]
        public string LastName { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Partner Number is required")]
        [StringLength(20, ErrorMessage = "Partner Number must be exactly 20 characters long")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Field must be numeric")]
        public string PartnerNumber { get; set; }

        [StringLength(11, ErrorMessage = "Croatian PIN must be exactly 11 characters long")]
        public string CroatianPIN { get; set; }

        [Required(ErrorMessage = "Partner type Id is required")]
        public string PartnerTypeId { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        [Required(ErrorMessage = "User email is required")]
        [StringLength(maximumLength: 255, ErrorMessage = "User email must be maximum 255 characters")]
        public string CreateByUser { get; set; }

        [Required(ErrorMessage = "Is foreign field is required")]
        public bool IsForeign { get; set; }

        [Required(ErrorMessage = "External code is required")]
        [StringLength(maximumLength: 20, MinimumLength = 10, ErrorMessage = "External Code must be between 10 and 20 characters")]
        public string ExternalCode { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public char Gender { get; set; }
    }
}