using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Models
{
    public class Teacher
    {
        public string MemberId { get; set; }
        public virtual Member Member { get; set; }
        public string Department { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
