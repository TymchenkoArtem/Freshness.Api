using Freshness.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly FreshnessContext _context;
        private DbSet<T> _entities;
        private bool disposedValue = false;

        public DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }

                return _entities;
            }
        }

        public Repository(FreshnessContext context)
        {
            _context = context;
        }

        public async Task<T> InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var addedEntity = await Entities.AddAsync(entity);

            return addedEntity.Entity;
        }

        public T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var updatedEntity = Entities.Update(entity);

            return updatedEntity.Entity;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var entity = await Entities.FindAsync(id);

            if (entity == null)
            {
                return false;
            }

            Entities.Remove(entity);

            return true;
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entities.Where(predicate).ToListAsync();
        }

        public async Task<(List<T>, int)> GetAsync(int offset, int limit)
        {
            return (await Entities.Skip(offset).Take(limit).ToListAsync(), await Entities.CountAsync());
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entities.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entities.AnyAsync(predicate);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                _context.Dispose();
                _entities = null;
            }

            disposedValue = true;
        }

        ~Repository()
        {
            Dispose(false);
        }
    }
}
