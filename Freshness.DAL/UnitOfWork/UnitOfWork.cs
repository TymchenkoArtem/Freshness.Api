using Freshness.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freshness.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly FreshnessContext _context;
        private Dictionary<Type, object> _repositories;
        private bool _disposedValue = false;

        public UnitOfWork(IServiceProvider serviceProvider, FreshnessContext context)
        {
            _serviceProvider = serviceProvider;
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories.Keys.Contains(typeof(T)))
            {
                return _repositories[typeof(T)] as IRepository<T>;
            }

            IRepository<T> repository = _serviceProvider.GetRequiredService<IRepository<T>>();

            _repositories.Add(typeof(T), repository);

            return repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing)
            {
                _context.Dispose();
            }

            _disposedValue = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

    }
}
