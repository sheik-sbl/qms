using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class Monthlyswservice
    {
        public int Id { get; set; }
        public string AgentName { get; set; }
        public string CallsAudited { get; set; }
        public string TotalScore { get; set; }
        public string QualityScore { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }
        public double budgeINR { get; set; }
        public double ActualINR { get; set; }
        public decimal cbacklog { get; set; }
        public decimal cumbacklog { get; set; }
        public List<Monthlyswservice> LstMonthlyswservice { get; set; }
    }
}