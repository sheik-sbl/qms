using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class TargetrevenueModel
    {
        public string project { get; set; }
        public double TarrevenueINR { get; set; }
        public double ActrevenueINR { get; set; }
        public double RevAchievement { get; set; }
        public List<TargetrevenueModel> LstTargetrevenueModel { get; set; }
    }
}