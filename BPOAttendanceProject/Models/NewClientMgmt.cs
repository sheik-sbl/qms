using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class NewClientMgmt
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int MMSId { get; set; }
        public string MMS { get; set; }
        public int BPOId { get; set; }
        public string BPOOnline { get; set; }
        public int CallId { get; set; }
        public string Call { get; set; }
        public List<NewClientMgmt> LstClientMgmt { get; set; }
    }
}