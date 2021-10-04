using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class InvoiceModel
    {
        public int Id { get; set; }
        public string MonthName { get; set; }
        public string YearName { get; set; }
        public double Target { get; set; }
        public double Actual { get; set; }
        public double Achievement { get; set; }
        public List<InvoiceModel> InvoiceModelList { get; set; }
    }
}