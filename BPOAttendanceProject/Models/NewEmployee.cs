using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class NewEmployee
    {
        public int Id { get; set; }
        public string PSN { get; set; }
        public string Associatename { get; set; }
        public string Location { get; set; }
        public string DOJ { get; set; }
        public bool IsActive { get; set; }
        public List<NewEmployee> EmployeeList { get; set; }
    }
}