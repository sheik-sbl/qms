using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class ReasonModel
    {
        public int id { get; set; }
        public string date { get; set; }
        public string Remarks { get; set; }
        public string location { get; set; }
        public int Days { get; set; }
        public List<ReasonModel> ReasonModelList { get; set; }
    }
}