using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BPOAttendanceProject.Models
{
    public class SlowModel
    {
      
            public List<TestEmployee> LstEmployee { get; set; }
            public IEnumerable<SelectListItem> ProjconfList { get; set; }
       
    }
}