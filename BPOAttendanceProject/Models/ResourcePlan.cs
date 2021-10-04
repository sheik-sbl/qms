using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class ResourcePlan
    {
        public int Id { get; set; }
        public string Projectcode { get; set; }
        public string eventcode { get; set; }
        public string Startdate { get; set; }
        public string Completiondate { get; set; }
        public double TotaltargetP { get; set; }
        public double Completiontarget { get; set; }
        public double Immediatetarget { get; set; }
        public double Totalcharactersavailable { get; set; }
        public int Holiday { get; set; }
        public string Date { get; set; }
        public List<ResourcePlan> ResourcePlanList { get; set; }
    }
}






















  