using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WeatherApp.Web.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Create(TEntity item);
        TEntity Get(string id);
        TEntity Get(Func<TEntity, bool> id, params Expression<Func<TEntity, object>>[] includeProperies);
        IEnumerable<TEntity> GetAll();
        void Delete(TEntity item);
        void Update(TEntity item);
    }
}
