using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class StaffAugServices
    {
      

            public int Id { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
            public string softwareservices { get; set; }
            public double internalbilling { get; set; }
            public double externalbilling { get; set; }
            public double Total { get; set; }
            public double etoinINR { get; set; }
            public double etoinUSD { get; set; }
            public double Resources { get; set; }
            public string Attrition { get; set; }
            public double billedincluded { get; set; }
            public double notbilled { get; set; }
            public string bestperformer { get; set; }
            public double idlehrs { get; set; }
            public List<StaffAugServices> LstStaffAugServices { get; set; }

    }
}