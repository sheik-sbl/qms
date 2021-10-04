using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class EmployeeETO
    {
        //public string Date { get; set; }
        //public int Id { get; set; }
        //public string Location { get; set; }
        //public string Projectcode { get; set; }
        public string psn { get; set; }
        public string associate { get; set; }
        public double actualrevenue { get; set; }
        public double ETOActualrevenue { get; set; }
        public double dollarrate { get; set; }
        public string Date { get; set; }
        public List<EmployeeETO> LstEmployeeETO { get; set; }
    }
}