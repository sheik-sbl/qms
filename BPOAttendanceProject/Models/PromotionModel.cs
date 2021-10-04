using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class PromotionModel
    {
            public int Id { get; set; }
          
            public string project { get; set; }
            public double noofbatches { get; set; }
            public double totalpromotion { get; set; }
            public double ratecharacter { get; set; }
            public string Location { get; set; }
            public string proddate { get; set; }
            public List<PromotionModel> PromotionModelList { get; set; }

    }
}