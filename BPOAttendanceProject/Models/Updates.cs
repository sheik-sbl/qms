using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class Updates
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Comments { get; set; }
        public List<Updates> LstUpdates { get; set; }
    }
}