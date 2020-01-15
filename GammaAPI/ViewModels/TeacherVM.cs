using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GammaAPI.ViewModels
{
    public class TeacherVM
    {
        public string MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string SchoolId { get; set; }
        public bool IsSelected { get; set; }
    }
}
