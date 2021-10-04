using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class Teamwisemrm
    {
       
            public int Id { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
            public string  empname { get; set; }
            public List<Teamwisemrm> LstTeamwisemrm { get; set; }
        }
    }
