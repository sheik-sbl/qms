using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class PecodeModel
    {
        public int Id { get; set; }
        public string pecode { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public List<PecodeModel> PecodeModelList { get; set; }
    }
}