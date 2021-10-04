using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class chartdata
    {
        public string monthyear { get; set; }
        public double BudgetINR { get; set; }
        public double ActualINR { get; set; }
        public double Percent { get; set; }
        public double Backlog { get; set; }
    }
}