using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class MonthlyStaffAugTarget
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public double target { get; set; }
        public List<MonthlyStaffAugTarget> LstMonthlyStaffAugTarget { get; set; }
    }
}