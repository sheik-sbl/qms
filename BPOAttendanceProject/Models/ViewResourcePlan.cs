using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class ViewResourcePlan
    {
       
            public int Id { get; set; }
            public string Projectdate { get; set; }
            public string Projectcode { get; set; }
            public string eventcode { get; set; }
            public string Startdate { get; set; }
            public string Completiondate { get; set; }
            public double Totaltarget { get; set; }
            public double TotaltargetP { get; set; }
            public double Completiontarget { get; set; }
            public string Referencedate { get; set; }
            public double AchievetillRefdate { get; set; }
            public int Holiday { get; set; }
            public int remainday { get; set; }
            public double balanceAchieve { get; set; }
            public double balanceAchieveday { get; set; }
            public double Indexingtarget { get; set; }
            public int Noofhrsreqdday { get; set; }
            public int Noofassociatereqday { get; set; }
            public int  todayachieve { get; set; }
            public int todayachievepercent { get; set; }
            public int associatedeployed { get; set; }
            public string DeviationReason { get; set; }
            public double ActualCharacters { get; set; }
            public string TodayDate { get; set; }
       

    }
}