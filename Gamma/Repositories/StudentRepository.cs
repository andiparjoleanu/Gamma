using Gamma.Context;
using Gamma.IRepositories;
using Gamma.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Repositories
{
    class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(GammaContext context) : base(context) { }
    }
}
