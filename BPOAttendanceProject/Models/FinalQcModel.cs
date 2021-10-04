using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class FinalQcModel
    {
      
        public int Id { get; set; }
        public string Location { get; set; }
        public string project { get; set; }
        public double noofbatches{ get; set; }
        public double totalpromotion { get; set; }
        public double characterrate { get; set; }
        public string proddate { get; set; }
        public string TL { get; set; }
        public string Eventcode { get; set; }
        public string Clientcode { get; set; }
        public List<FinalQcModel> QcModelList { get; set; }
    
    }
}