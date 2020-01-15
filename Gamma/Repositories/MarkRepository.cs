using Gamma.Context;
using Gamma.IRepositories;
using Gamma.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Repositories
{
    public class MarkRepository : BaseRepository<Mark>, IMarkRepository
    {
        public MarkRepository(GammaContext context) : base(context) { }
    }
}
