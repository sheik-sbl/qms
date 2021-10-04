using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
   


    public class MailModel
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public bool IsChecked { get; set; }
    }
    public class Maillist
    {
        public List<MailModel> CheckBoxItems { get; set; }
    }


}