using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class MonthlyMMSTarget
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public double target { get; set; }
        public double mmstarget { get; set; }
        public double bpotarget { get; set; }
        public double calltarget { get; set; }
        public List<MonthlyMMSTarget> LstMonthlyMMSTarget { get; set; }
    }
}