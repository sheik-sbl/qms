using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class teamwiseitem
    {
        public int Id { get; set; }
        public string weekinmonth { get; set; }
        public double Empcount { get; set; }
        public double Billablehrs { get; set; }
        public double Externalhrs { get; set; }
        public double Appinternalhrs { get; set; }
        public double unbilledhrs { get; set; }
        public List<TeamwiseModel> teamwiseDetails { get; set; }
    }
}