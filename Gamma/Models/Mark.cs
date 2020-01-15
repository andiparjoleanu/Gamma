using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Models
{
    public class Mark
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public string Note { get; set; }
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public virtual StudentCourse StudentCourse { get; set; }
    }
}
