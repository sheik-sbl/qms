using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class TeamwiseChart
    {
        public string name { get; set; }
        public double Employeecount { get; set; }
        public double BillableHrs { get; set; }
        public double ExternalBilledHrs { get; set; }
        public double InternalProjectHrs { get; set; }
        public double UnbilledHrs { get; set; }
    }
}