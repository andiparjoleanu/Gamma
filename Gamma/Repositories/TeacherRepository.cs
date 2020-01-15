using Gamma.Context;
using Gamma.IRepositories;
using Gamma.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Repositories
{
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(GammaContext context) : base(context) { }
    }
}
