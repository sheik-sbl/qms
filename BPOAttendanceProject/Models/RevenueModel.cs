using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class RevenueModel
    {
           public DateTime date { get; set; }
           public double Targetrevenue { get; set; }
           public double Actualrevenue { get; set; }
           public double Achievement { get; set; }
           public double Tarmonth { get; set; }
           public List<RevenueModel> RevenueModelList { get; set; }

    }
}