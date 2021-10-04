using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class OpenCpyconfigModel
    {
        public int id { get; set; }
        public string FromMonthName { get; set; }
        public string ToMonthName { get; set; }
        public int FromMonthId { get; set; }
        public int ToMonthId { get; set; }
        public string FromYear { get; set; }
        public string ToYear { get; set; }
        public List<OpenCpyconfigModel> FromMonthList { get; set; }
        public List<OpenCpyconfigModel> ToMonthList { get; set; }
    }
}