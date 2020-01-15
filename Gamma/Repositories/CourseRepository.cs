using Gamma.Context;
using Gamma.IRepositories;
using Gamma.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Repositories
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(GammaContext context) : base(context) { }
    }
}
