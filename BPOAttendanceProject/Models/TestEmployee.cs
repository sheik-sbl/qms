using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class TestEmployee
    {
        public int Id { get; set; }
        public string PSN { get; set; }
        public string Associatename { get; set; }
        public List<TestEmployee> EmployeeList { get; set; }
    }
}