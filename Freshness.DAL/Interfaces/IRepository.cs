using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.DAL.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        Task<T> InsertAsync(T entity);

        T Update(T entity);

        Task<bool> RemoveAsync(int id);

        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);

        Task<(List<T>, int)> GetAsync(int offset, int limit);

        Task<T> FindAsync(Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
