using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class AttendanceModel
    {
        public int id { get; set; }
        public string date { get; set; }
        public string psn { get; set; }
        public string name { get; set; }
        public string tl { get; set; }
        public string attendance { get; set; }
        public string project { get; set; }
    }
}