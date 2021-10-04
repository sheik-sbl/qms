using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class MonthlyConfiguration
    {

        public int Id { get; set; }
        public int monthid { get; set; }
        public int Configuration { get; set; }
        public string monthname { get; set; }
        public string location { get; set; }
        public int locationId { get; set; }
        public string year { get; set; }
        public double Revenueconfiguration { get; set; }
        public int workingdays { get; set; }
        
        public List<MonthlyConfiguration> MonthConfList { get; set; }
    }
}