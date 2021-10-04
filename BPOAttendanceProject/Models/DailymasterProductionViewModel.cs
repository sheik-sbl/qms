using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class DailymasterProductionViewModel
    {
        public int psn { get; set; }
        public string associate { get; set; }
        public string Experience { get; set; }
        public string process { get; set; }
        public string project { get; set; }
        public string projectcode { get; set; }
        public string eventcode { get; set; }
        public string tlname { get; set; }
        public int plannedhrs { get; set; }
        public int plannedhrrecord { get; set; }
        public int plannedprodrecord { get; set; }
        public double workedhrs { get; set; }
        public int actualprodrecord { get; set; }
        public int achievement { get; set; }
        public string remarks { get; set; }
        public string location { get; set; }
        public string  date { get; set; }
        public double targetrevenue { get; set; }
        public double actualrevenue { get; set; }
        public double revenueachievement { get; set; }
        public double workathome { get; set; }
        public List<DailymasterProductionViewModel> LstDailymasterProductionReport { get; set; }
    }
}