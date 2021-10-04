using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class SummaryReport
    {
        public int Totalassociatesworked { get; set; }
        public int Totalassociatehours { get; set; }
        public int Totalrecordsprocessed { get; set; }
        public int Totalcharactersdelivered { get; set; }
        public double Averagerecordshour { get; set; }
        public double Averagecharactersdelivered { get; set; }

    }
}