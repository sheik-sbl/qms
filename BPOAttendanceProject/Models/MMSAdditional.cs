using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class MMSAdditional
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Attrition { get; set; }
        public int TeamSize { get; set; }
        public int Resigned { get; set; }
        public double Target { get; set; }
        public double Actual { get; set; }
        //public string Department { get; set; }
        public int Size { get; set; }
        public int MMSSize { get; set; }
        public double ETOINR { get; set; }
        public double ETOUSD { get; set; }
        public int Incentive { get; set; }
        public double Billable { get; set; }
        public double NonBillable { get; set; }
        public double MMSBillable { get; set; }
        public double MMSNonBillable { get; set; }
        public double MMSETOINR { get; set; }
        public double MMSETOUSD { get; set; }
        public double BPOETOINR { get; set; }
        public double BPOETOUSD { get; set; }
        public string  MMSComments { get; set; }
        public string BPOComments { get; set; }
        public double ETO { get; set; }
        public int CallSize { get; set; }
        public double CallBillable { get; set; }
        public double CallNonBillable { get; set; }
        public double CallETOINR { get; set; }
        public double CallETOUSD { get; set; }
        public string CallComments { get; set; }
        public List<MMSAdditional> LstMMSAdditional { get; set; }

    }
}