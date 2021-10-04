using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class Userlogin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public int Roleid { get; set; }
        
    }
}