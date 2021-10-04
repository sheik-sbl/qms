using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class TeamleadProductionViewModel
    {
        public int psn { get; set; }
        public string associate { get; set; }
        public string Experience { get; set; }
        public string process { get; set; }
        public string project { get; set; }
        public string projectcode { get; set; }
        public string eventcode { get; set; }
        public string tlname { get; set; }
        public double hoursplanned { get; set; }
        public double plannedhrrecord { get; set; }
        public double plannedprodrecord { get; set; }
        public string hoursworked { get; set; }
        public double actualproduction { get; set; }
        public double achievement { get; set; }
        public string remarks { get; set; }
        public string location { get; set; }
        public string date { get; set; }
        public double targetrevenue { get; set; }
        public double actualrevenue { get; set; }
        public double revenueachievement { get; set; }
        public List<TeamleadProductionViewModel> LstTeamleadProductionReport { get; set; }
    }
}