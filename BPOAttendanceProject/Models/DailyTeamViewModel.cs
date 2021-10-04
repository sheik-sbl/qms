using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BPOAttendanceProject.Models
{
    public class DailyTeamViewModel
    {
        public string Date { get; set; }
        public int Id { get; set; }
        public List<User> UserList { get; set; }
    }
}