using Gamma.Context;
using Gamma.IRepositories;
using Gamma.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Repositories
{
    public class StudentCourseRepository : BaseRepository<StudentCourse>, IStudentCourseRepository
    {
        public StudentCourseRepository(GammaContext context) : base(context) { }
    }
}
