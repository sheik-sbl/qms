using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class RevenueConfiguration
    {

        public int Id { get; set; }
        public string Projectcode { get; set; }
        public string Eventcode { get; set; }
        public double Price { get; set; }
        public double Indexing { get; set; }
        public double Qc2 { get; set; }
        public double Qc3 { get; set; }
        public double UAT { get; set; }
        public double Audit { get; set; }
        public double Rework { get; set; }
        public List<RevenueConfiguration> RevenueConfList { get; set; }
        public bool IsActive { get; set; }
    }
}