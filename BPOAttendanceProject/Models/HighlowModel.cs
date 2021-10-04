using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class HighlowModel
    {
        public string pecode { get; set; }
        public string projectcode { get; set; }
        public string eventcode { get; set; }
        public string highlocation { get; set; }
        public string highdate { get; set; }
        public double highproduction { get; set; }
        public string lowlocation { get; set; }
        public string lowdate { get; set; }
        public double lowproduction { get; set; }
        public List<HighlowModel> HighlowModelList { get; set; }
    }
}