using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class MonthlyRecord
    {
        public string Monthname { get; set; }
        public double targetrevenue { get; set; }
        public double actualrevenue { get; set; }
        public List<MonthlyRecord> LstMonthrecord { get; set; }
    }
}