using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace WeatherApp.Data.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        DbContext _context;
        DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public void Create(TEntity item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }
        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public TEntity Get(string id)
        {
            return _dbSet.Find(id);
        }

        public TEntity Get(Func<TEntity, bool> id, params Expression<Func<TEntity, object>>[] includeProperies)
        {
            var results = _dbSet.Include(includeProperies[0]);
            for (int i = 1; i < includeProperies.Length; i++)
            {
                results = _dbSet.Include(includeProperies[i]);
            }
            return results.First(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Delete(TEntity item)
        {
            _dbSet.Remove(item);
            _context.SaveChanges();
        }
    }
}
