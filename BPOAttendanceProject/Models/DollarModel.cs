using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class DollarModel
    {
        public int id { get; set; }
       
        public string dollardate { get; set; }
        [Required]
        public double dollarrate { get; set; }
        [Required]
        public double poundrate { get; set; }
        public List<DollarModel> DollarList { get; set; }
    }
}