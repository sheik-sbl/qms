using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class RevenuelistModel
    {
        public int Id { get; set; }
        public string projectcode { get; set; }
        public string eventcode { get; set; }
        public double noofbatches { get; set; }
        public double invoicedcharacter { get; set; }
        public double pendingcharacter { get; set; }
        public double pendingp2 { get; set; }
        public string Location { get; set; }
        public double Total { get; set; }
        public string upldate { get; set; }
        public string clientcode { get; set; }
        public string batchname { get; set; }
        public string RO { get; set; }
        public double actualprodrecord { get; set; }
        public double promotionrecord { get; set; }
        public List<RevenuelistModel> RevenueModelList { get; set; }
    }
}