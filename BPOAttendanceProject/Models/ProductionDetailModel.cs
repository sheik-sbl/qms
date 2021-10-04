using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class ProductionDetailModel
    {
        public string projectcode { get; set; }
        public string eventcode { get; set; }
        public string date { get; set; }
        public double plannedprodrecord { get; set; }
        public double actualprodrecord { get; set; }
        public double Totalcharacters { get; set; }
        public List<ProductionDetailModel> lstproductionDetail { get; set; }

    }
}