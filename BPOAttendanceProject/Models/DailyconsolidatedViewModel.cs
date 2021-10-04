using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class DailyconsolidatedViewModel
    {

                public string Date {get;set;}
                public string Location {get;set;}
                public double hoursplanned {get;set;}
                public double prodplanhrRecord {get;set;}
                public double prodplanRecord {get;set;}
                public double RecordsHours {get;set;}
                public double ActualProdRecords {get;set;}
                public double Achievement {get;set;}
                public double TargetRevenue {get;set;} 
                public double ActualRevenue {get;set;}
                public double RevenueAchievement { get; set; }
                public List<DailyconsolidatedViewModel> LstDailyconsolidated { get; set; }

    }
}