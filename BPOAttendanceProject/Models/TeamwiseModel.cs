using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class TeamwiseModel
    {
        public int teamwiseid { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string empname { get; set; }

        private List<teamwiseitem> lstItems = new List<teamwiseitem>();

        public List<teamwiseitem> LstItems
        {
            get { return lstItems; }
            set { lstItems = value; }
        }


    }
}