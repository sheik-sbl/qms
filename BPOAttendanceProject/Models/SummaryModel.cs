using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BPOAttendanceProject.Models
{
    public class SummaryModel
    {

        public List<Employee> EmployeeList { get; set; }
        public string projectcode { get; set; }
        public string eventcode { get; set; }
        public IEnumerable<SelectListItem> ProjconfList { get; set; }
        public IEnumerable<SelectListItem> EventList { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }

    }
}