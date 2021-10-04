using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class NotexistModel
    {
        public int id { get; set; }
        public string Projectcode { get; set; }
        public string Eventcode { get; set; }
        public string Process { get; set; }
        public string location { get; set; }
        public string prtype { get; set; }
        public List<NotexistModel> NotexistList { get; set; }
        public int cnt { get; set; }

    }
}