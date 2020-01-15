using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GammaAPI.ViewModels
{
    public class StudentVM
    {
        public string MemberId { get; set; }
        public int Grade { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FieldOfStudy { get; set; }
        public string SchoolId { get; set; }
        public bool IsSelected { get; set; }
    }
}
