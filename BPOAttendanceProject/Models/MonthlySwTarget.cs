using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class MonthlySwTarget
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public double target { get; set; }
        public List<MonthlySwTarget> LstMonthlySwTarget { get; set; }
    }
}