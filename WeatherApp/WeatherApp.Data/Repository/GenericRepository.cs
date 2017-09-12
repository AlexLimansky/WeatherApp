using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace WeatherApp.Data.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbContext context;
        private DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public void Create(TEntity item)
        {
            this.dbSet.Add(item);
            this.context.SaveChanges();
        }

        public void Update(TEntity item)
        {
            this.context.Entry(item).State = EntityState.Modified;
            this.context.SaveChanges();
        }

        public TEntity Get(string id)
        {
            return this.dbSet.Find(id);
        }

        public TEntity Get(Func<TEntity, bool> id, params Expression<Func<TEntity, object>>[] includeProperies)
        {
            var results = this.dbSet.Include(includeProperies[0]);
            for (int i = 1; i < includeProperies.Length; i++)
            {
                results = this.dbSet.Include(includeProperies[i]);
            }

            return results.First(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.dbSet.ToList();
        }

        public void Delete(TEntity item)
        {
            this.dbSet.Remove(item);
            this.context.SaveChanges();
        }
    }
}