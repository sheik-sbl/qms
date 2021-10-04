using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class TLproductivityModel
    {
        public string tlname { get; set; }
        public double Plannedproduction { get; set; }
        public double Actualproduction { get; set; }
        public double Achievement { get; set; }
        public double Productivity { get; set; }
        public List<BPOAttendanceProject.Models.TLproductivityModel> LstTLproductivityModel { get; set; }
    }
}