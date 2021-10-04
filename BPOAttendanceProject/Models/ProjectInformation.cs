using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class ProjectInformation
    {
        public int id { get; set; }
        public string projectdate { get; set; }
        public string custId { get; set; }
        public string custname { get; set; }
        public string businesstype { get; set; }
        public string agreement { get; set; }
        public string agreementdate { get; set; }
        public string agreementname { get; set; }
        public int projid { get; set; }
        public string projectname { get; set; }
        public string projectcode { get; set; }
        public string projawarddate { get; set; }
        public string sowsigned { get; set; }
        public string estprojbillingunit { get; set; }
        public double esttotalvolume { get; set; }
        public double acttotalvolume { get; set; }
        public double estunitrate { get; set; }
        public string estcurrency { get; set; }
        public double estamount { get; set; }
        public double actamount { get; set; }
        public double estamountUSD { get; set; }
        public double actamountUSD { get; set; }
        public double estamountINR { get; set; }
        public double actamountINR { get; set; }
        public string eststartdate { get; set; }
        public string actstartdate { get; set; }
        public string estenddate { get; set; }
        public string actenddate { get; set; }
        public double estavgindxprdn { get; set; }
        public double actavgindxprdn { get; set; }
        public double estavgqcprdn { get; set; }
        public double actavgqcprdn { get; set; }
        public int actInhouse_1_KPLY { get; set; }
        public int estInhouse_1_KPLY { get; set; }
        public int actInhouse_1_KOCHI { get; set; }
        public int estInhouse_1_KOCHI { get; set; }
        public int actInhouse_3_MDR { get; set; }
        public int estInhouse_3_MDR { get; set; }
        public int actInhouse_4_TVM { get; set; }
        public int estInhouse_4_TVM { get; set; }
        public int actInhouse_5_WFH { get; set; }
        public int estInhouse_5_WFH { get; set; }
        public int actInhouse_6_Partner { get; set; }
        public int estInhouse_6_Partner { get; set; }
        public int estqcInhouse_1_KPLY { get; set; }
        public int actqcInhouse_1_KPLY { get; set; }
        public int estqcInhouse_2_KOCHI { get; set; }
        public int actqcInhouse_2_KOCHI { get; set; }
        public int estqcInhouse_3_MDR { get; set; }
        public int actqcInhouse_3_MDR { get; set; }
        public int estqcInhouse_4_TVM { get; set; }
        public int actqcInhouse_4_TVM { get; set; }
        public double estaverageeto { get; set; }
        public double actaverageeto { get; set; }
        public double exectarincentive { get; set; }
        public double execacttargetincentive { get; set; }
        public double targetvendorcharge { get; set; }
        public double actualvendorcharge { get; set; }
        public string coastapproved { get; set; }
        public string coastapproveddate { get; set; }
        public string coastvendorapproved { get; set; }
        public string coastvendordate { get; set; }
        public string approvalpreparedname { get; set; }
        public string approvalprepareddate { get; set; }
        public string approvalpreparedGM { get; set; }
        public string approvalpreparedGMdate { get; set; }
        public string approvalpreparedhites { get; set; }
        public string approvalpreparedhitesdate { get; set; }
        public string approvalpreparedfinancename { get; set; }
        public string approvalpreparedfinancedate { get; set; }
        public string outputpath { get; set; }
        public string additionalnote { get; set; }
        public string mailnotification { get; set; }
        public string comments { get; set; }
        public string ceoapproval { get; set; }
        public string Inhouse_001_KNPY { get; set; }
        public string Inhouse_002_KOCHI { get; set; }
        public string Inhouse_003_MDR { get; set; }
        public string Inhouse_004_TVM { get; set; }
        public int ceomail { get; set; }
        public int financemail { get; set; }
        public int headitesmail { get; set; }
        public int gmmail { get; set; }
        public int am { get; set; }
        public int pm { get; set; }
        public string cusprefix { get; set; }

       
        private List<ProjectMilestoneItem> lstItems = new List<ProjectMilestoneItem>();

        public List<ProjectMilestoneItem> LstItems
        {
            get { return lstItems; }
            set { lstItems = value; }
        }


      


    }
}