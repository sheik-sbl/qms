using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Utility
{
    public class CurrentSession
    {
        public static string MonthconfigurationCount
        {
            get;
            set;
        }
        public static List<string> location;

        public static string teamleadCount
        {
            get;
            set;
        }

        public static string notifycount
        {
            get;
            set;
        }

        public static List<string> Teamlead;
    }
}