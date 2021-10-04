using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class PeriodicProductionViewModel
    {
        public string Date { get; set; }
        public int Id { get; set; }
        public List<User> UserList { get; set; }
        public List<BPOAttendanceProject.Models.DailyTLProduction> LstDailyProductionReport { get; set; }
        public List<BPOAttendanceProject.Models.DailyTLwiseProduction> LstDailyTLwiseProductionReport { get; set; }
    }
}