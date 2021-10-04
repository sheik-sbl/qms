using BPOAttendanceProject.Customvalidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class MyViewModel
    {
        [Required(ErrorMessage = "Please Enter Your Full Name")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Address")]
        [Display(Name = "Address")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Upload File")]
        [Display(Name = "Upload File")]
        [ValidateFile]
        public HttpPostedFileBase file { get; set; }
    }
}