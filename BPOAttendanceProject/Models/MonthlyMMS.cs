using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class MonthlyMMS
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public double budgeINR { get; set; }
        public double ActualINR { get; set; }
        public double MMSbudgeINR { get; set; }
        public double MMSActualINR { get; set; }
        public double ONbudgeINR { get; set; }
        public double ONActualINR { get; set; }
        public double CallbudgeINR { get; set; }
        public double CallActualINR { get; set; }
        public double Achievement { get; set; }
        public string Comments { get; set; }
        public decimal cbacklog { get; set; }
        public decimal cumbacklog { get; set; }
        public decimal onbacklog { get; set; }
        public decimal callbacklog { get; set; }
        public List<MonthlyMMS> LstMonthlyMMS { get; set; }
    }
}