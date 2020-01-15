using Gamma.Context;
using Gamma.IRepositories;
using Gamma.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Repositories
{
    public class SchoolRepository : BaseRepository<School>, ISchoolRepository
    {
        public SchoolRepository(GammaContext context) : base(context) { }
    }
}
