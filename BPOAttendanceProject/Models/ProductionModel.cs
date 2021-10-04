using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class ProductionModel
    {
        public string projectcode { get; set; }
        public string eventcode { get; set; }
        //public string date { get; set; }
        public double plannedprodrecord { get; set; }
        public double actualprodrecord { get; set; }
        public double workedhrs { get; set; }
        public int employeeno { get; set; }
        public List<BPOAttendanceProject.Models.ProductionModel> LstProductionModel { get; set; }

    }
}