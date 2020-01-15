using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Models
{
    public class Course
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TeacherId { get; set; }
        public int Grade { get; set; }
        public string FieldOfStudy { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
