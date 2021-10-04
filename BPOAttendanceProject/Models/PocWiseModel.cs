using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class PocWiseModel
    {
        
            public int Id { get; set; }
            public string MonthName { get; set; }
            public string YearName { get; set; }
            public string PocName { get; set; }
            public double Target { get; set; }
            public double Actual { get; set; }
            public double Achieved { get; set; }
            public List<PocWiseModel> PocWiseModelList { get; set; }
       
    }
}