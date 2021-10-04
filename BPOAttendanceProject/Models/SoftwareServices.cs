using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class SoftwareServices
    {
        public string DATE { get; set; }
        public string AGENTNAME { get; set; }
        public string CALLFROM { get; set; }
        public string CALLTO { get; set; }
        public string TicketNumber { get; set; }
        public string RecordingURL { get; set; }
        public string CALLREVIEW { get; set; }
        public string TICKETREVIEW { get; set; }
        public string Greeting { get; set; }
        public string REMARKS { get; set; }
        public string Probing { get; set; }
        public string REMARKS2 { get; set; }
        public string Tagging { get; set; }
        public string REMARKS3 { get; set; }
        public string Details { get; set; }
        public string REMARKS4 { get; set; }
        public string Solution { get; set; }
        public string REMARKS5 { get; set; }
        public string reminder { get; set; }
        public string REMARKS6 { get; set; }
        public string Timeline { get; set; }
        public string REMARKS8 { get; set; }
        public string listening { get; set; }
        public string REMARKS9 { get; set; }
        public string Phone { get; set; }
        public string REMARKS10 { get; set; }
        public string Grammar { get; set; }
        public string REMARKS11 { get; set; }
        public string Professionalism { get; set; }
        public string REMARKS12 { get; set; }
        public string tools { get; set; }
        public string Closing { get; set; }
        public string RemarksClosing { get; set; }
        public string rude { get; set; }
        public string Tagging2 { get; set; }
        public string mistakes { get; set; }
        public string Total { get; set; }
        public string ActionTaken { get; set; }


        //Not Required
        public int Id { get; set; }
        public string Month  { get; set; }
        public string Year { get; set; }
        public string softwareservices { get; set; }
        public double internalbilling { get; set; }
        public double externalbilling { get; set; }
        
        public double etoinINR { get; set; }
        public double etoinUSD { get; set; }
        public double Resources { get; set; }
        public string Attrition { get; set; }
        public double billedincluded { get; set; }
        public double notbilled { get; set; }
        public string bestperformer { get; set; }
        public double Idlehrs { get; set; }
        // not Required
        public List<SoftwareServices> LstSoftwareServices { get; set; }
        public List<NewClientMgmt> LstAgent { get; set; }
    }
}