using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class ViewPlanModel
    {
       
        public int Id { get; set; }
        public string Projectcode { get; set; }
        public string eventcode { get; set; }
        public string Startdate { get; set; }
        public string Completiondate { get; set; }
        public double immtargetP { get; set; }
        public string referencedate { get; set; }
        public double Achtillrefdate { get; set; }
        public int Holiday { get; set; }
        public int remainday { get; set; }
        public double balanceAch { get; set; }
        public double balanceAchperday { get; set; }
        public int indexingtarget { get; set; }
        public int hrsreqday { get; set; }
        public int associatereqday { get; set; }
        public int todayachievement { get; set; }
        public int todayperachieve { get; set; }
        public int associatesdeployed { get; set; }
        public string reasondeviation { get; set; }
        public double actualcharacter { get; set; }
        public List<ViewPlanModel> ViewPlanModelList { get; set; }
    }
}