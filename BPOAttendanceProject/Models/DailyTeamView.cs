using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class DailyTeamView
    {
        public List<BPOAttendanceProject.Models.DailyProduction> LstDailyTeamReport { get; set; }
        public SummaryReport Summaryinfo { get; set; }
    }
}