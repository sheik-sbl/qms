using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class DailyETO
    {
        public string Date { get; set; }
        public int Id { get; set; }
        public string Location { get; set; }
        public string Projectcode { get; set; }
        public double Actualrevenue { get; set; }
        public double ETOActualrevenue { get; set; }
        public double Employeeno { get; set; }
        public double dollarrate { get; set; }
        public List<DailyETO> LstDailyETO { get; set; }
    }
}