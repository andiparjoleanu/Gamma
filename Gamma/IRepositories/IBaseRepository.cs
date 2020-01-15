using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gamma.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task CreateAsync(T entity);
        void Delete(T entity);
        T Update(T entity);
    }
}
