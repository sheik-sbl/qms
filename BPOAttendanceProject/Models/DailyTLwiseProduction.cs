using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class DailyTLwiseProduction
    {
        public string date { get; set; }
        public string tl { get; set; }
        public int Associatecount { get; set; }
        //public string project { get; set; }
        public int totaltarget { get; set; }
        public int totalproduction { get; set; }
        public int totalhours { get; set; }
        public int Charactercount { get; set; }
        public double Averagerecordhour { get; set; }
        public double Averagecharacterhour { get; set; }
    }
}