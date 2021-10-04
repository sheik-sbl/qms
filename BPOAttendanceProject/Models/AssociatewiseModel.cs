using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class AssociatewiseModel
    {
        public string associate { get; set; }
        public string Date { get; set; }
        public string projectcode { get; set; }
        public string eventcode { get; set; }
        public string Process { get; set; }
        public double plannedprodrecord { get; set; }
        public double actualprodrecord { get; set; }
        public double workedhrs { get; set; }
        public double Productivity { get; set; }
        public List<BPOAttendanceProject.Models.AssociatewiseModel> LstAssociatewiseModel { get; set; }
    }
}