using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Models
{
    public class Member : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DesiredRole { get; set; }
        public string SchoolId { get; set; }
        public virtual School School { get; set; }
        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
