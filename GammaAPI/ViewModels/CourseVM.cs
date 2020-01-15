using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GammaAPI.ViewModels
{
    public class CourseVM
    {
        public int Grade { get; set; }
        public string FieldOfStudy { get; set; }
        public string CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TeacherId { get; set; }
    }
}
