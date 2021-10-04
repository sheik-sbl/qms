using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class ProjectMilestoneItem
    {

        public int Id { get; set; }
        public string MilestoneName { get; set; }
        public string MilestoneDate { get; set; }
        public List<ProjectInformation> milestoneDetails { get; set; }



    }
}