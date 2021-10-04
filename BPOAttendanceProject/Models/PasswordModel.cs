using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class PasswordModel
    {
            public int userid { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
       
    }
}