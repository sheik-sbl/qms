using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string PSN { get; set; }
        public string Associatename { get; set; }
      
        public string project { get; set; }
        public string projectcode { get; set; }
        public string eventcode { get; set; }
        public string process { get; set; }
        public string hoursplanned { get; set; }
        public string hoursworked { get; set; }
       // public double ProductionPlannedHr { get; set; }
        public double ActualProduction { get; set; }
        public double workathome { get; set; }
        public string Leave { get; set; }
        public List<Employee> EmployeeList { get; set; }

    }
}