using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class PromotionDetailModel
    {
        public string project { get; set; }
        public string eventcode { get; set; }
        public string date { get; set; }
        public double batches { get; set; }
        public double promotion { get; set; }
        public string  location { get; set; }
        public double revenue { get; set; }
        public List<PromotionDetailModel> lstpromotionDetail { get; set; }
    }
}