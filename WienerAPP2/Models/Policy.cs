using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WienerAPP2.Models
{
    public class Policy
    {
        [Required(ErrorMessage = "External code is required")]
        [StringLength(maximumLength: 20, MinimumLength = 10, ErrorMessage = "External Code must be between 10 and 20 characters")]
        public string ExternalCode { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(maximumLength: 15, MinimumLength = 10, ErrorMessage = "Policy Number must be between 10 and 15 characters")]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Value is required")]
        public decimal Value { get; set; }
    }
}