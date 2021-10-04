using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class DailyProductionViewModel
    {
        public string Date { get; set; }
        public List<BPOAttendanceProject.Models.DailyTLProduction> LstDailyProductionReport { get; set; }
        public List<BPOAttendanceProject.Models.DailyTLwiseProduction> LstDailyTLwiseProductionReport { get; set; }
    }
}