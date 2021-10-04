using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class ProjectMgmt
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public int  Year { get; set; }
        public string clienttype { get; set; }
        public int clientId { get; set; }
        public string clientname { get; set; }
        public double Target { get; set; }
        public double Gained { get; set; }
        public double Achieved { get; set; }
        public List<ClientMgmt> LstClientMgmt { get; set; }
        public List<ProjectMgmt> LstProjectMgmt { get; set; }
    }
}