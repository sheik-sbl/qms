using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class ResourceproductivityModel
    {
        public string associate { get; set; }
        public double Targetrevenue { get; set; }
        public double Actualrevenue { get; set; }
        public double Productivity { get; set; }
        public double plannedprodrecord { get; set; }
        public double actualprodrecord { get; set; }
        public List<BPOAttendanceProject.Models.ResourceproductivityModel> LstResourceproductivityModel { get; set; }
    }
}