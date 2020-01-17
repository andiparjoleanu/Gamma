using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GammaAPI.ViewModels
{
    public class SubscribeStudentsVM
    {
        public List<StudentVM> Students { get; set; }
        public string CourseId { get; set; }
    }
}
