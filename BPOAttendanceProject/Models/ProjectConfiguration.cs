using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class ProjectConfiguration
    {
        public int Id { get; set; }
        public string Projectcode { get; set; }
        public string Eventcode { get; set; }
        public string Process { get; set; }
        public double ProductionPlannedHr { get; set; }
        public List<ProjectConfiguration> ProjectConfList { get; set; }
        public bool IsActive { get; set; }
        public int monthid { get; set; }
        public string monthname { get; set; }
        public string location { get; set; }
        public int locationId { get; set; }
        public string year { get; set; }
    }
}