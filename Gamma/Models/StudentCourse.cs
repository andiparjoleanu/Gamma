using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Models
{
    public class StudentCourse
    {
        public string StudentId { get; set; }
        public virtual Student Student { get; set; }
        public string CourseId { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<Mark> Marks { get; set; }
    }
}
