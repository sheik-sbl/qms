using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class DailyProduction
    {
        public int Id { get; set; }
        public string date { get; set; }
        public string psn { get; set; }
        public string name { get; set; }
        public string tl { get; set; }
        public string  attendance { get; set; }
        public string  project { get; set; }
        public int totaltarget { get; set; }
        public int totalproduction { get; set; }
        public int totalhours { get; set; }
        public int Charactercount { get; set; }
    }
}