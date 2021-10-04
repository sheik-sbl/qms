using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class MonthlyGisservice
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public double budgeINR { get; set; }
        public double ActualINR { get; set; }
        public decimal cbacklog { get; set; }
        public decimal cumbacklog { get; set; }
        //public double Inpercent { get; set; }
        //public double Backlog { get; set; }
        public List<MonthlyGisservice> LstMonthlyGisservice { get; set; }
    }
}