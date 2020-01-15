using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Models
{
    public class Student
    {
        public string MemberId { get; set; }
        public virtual Member Member { get; set; }
        public int Grade { get; set; }
        public string FieldOfStudy { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
