using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class ProjectproductivityModel
    {
        public string pecode { get; set; }
        public double plannedprodrecord { get; set; }
        public double actualprodrecord { get; set; }
        public double Targetrevenue { get; set; }
        public double Actualrevenue { get; set; }
        public double revenueachievement { get; set; }
        public double Productivity { get; set; }
        public List<BPOAttendanceProject.Models.ProjectproductivityModel> LstProjectproductivityModel { get; set; }
    }
}