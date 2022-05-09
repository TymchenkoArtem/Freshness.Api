using System;
using System.Threading.Tasks;

namespace Freshness.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;

        Task<int> SaveChangesAsync();
    }
}
