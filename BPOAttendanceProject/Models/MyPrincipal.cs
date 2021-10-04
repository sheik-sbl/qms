using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public class MyPrincipal : IPrincipal
    {

        public MyPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public IIdentity Identity
        {
            get;
            private set;
        }

        public Userlogin userlogin { get; set; }

        public bool IsInRole(string role)
        {
            return true;
        }

    }
}