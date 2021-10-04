using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class ClientMgmt
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string clienttype { get; set; }
        public string clientname { get; set; }
        public List<ClientMgmt> LstClientMgmt { get; set; }
    }
}