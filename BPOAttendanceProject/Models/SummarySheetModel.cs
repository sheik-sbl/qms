using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BPOAttendanceProject.Models
{
    public class SummarySheetModel
    {
        public string Date { get; set; }
        public string Location { get; set; }
        public double hoursplanned { get; set; }
        public double prodplanhrrecord { get; set; }
        public double prodplanrecords { get; set; }
        public double hoursworked { get; set; }
        public double Actualprodrecord { get; set; }
        public double Achievement { get; set; }
        public double TarrevenueINR { get; set; }
        public double ActrevenueINR { get; set; }
        public double RevAchievement { get; set; }
        public string projectcode { get; set; }
        public string eventcode { get; set; }
        public string process { get; set; }
        public int cnt { get; set; }
        public double Rate { get; set; }
        public int ETO { get; set; }
        public List<SummarySheetModel> lstSummarySheetmodel { get; set; }
       
    }
}