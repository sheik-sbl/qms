using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class Targetrevenue
    {
       
        public string month { get; set; }
        public string location { get; set; }
        public double targetrevenue { get; set; }
        public double actualrevenue { get; set; }
        public List<Targetrevenue> TargetrevenueList { get; set; }
    }
}