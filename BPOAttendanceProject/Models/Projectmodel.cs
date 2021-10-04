using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BPOAttendanceProject.Models
{
    public class Projectmodel
    {
        public string Date { get; set; }
        public int Id { get; set; }
        public string Location { get; set; }
        public string Projectcode { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public List<Projectmodel> ProjectModelList { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
    }
}