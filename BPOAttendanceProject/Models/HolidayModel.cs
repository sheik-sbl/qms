using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class HolidayModel
    {
        public int id { get; set; }
        public string holidaydate { get; set; }
        public string holidayname { get; set; }
        public string location { get; set; }
        public List<HolidayModel> HolidayList { get; set; }
    }
}