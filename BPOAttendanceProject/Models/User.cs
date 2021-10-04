using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPOAttendanceProject.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public int Status { get; set; }
        public string location { get; set; }
        public string Role { get; set; }
        public string PM { get; set; }
        public int locationId { get; set; }
        public string fullname { get { return this.FirstName + " " + this.LastName; } }
        public List<User> UserList { get; set; }

    }
}