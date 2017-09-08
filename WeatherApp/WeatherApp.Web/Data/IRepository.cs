using System.Collections.Generic;

namespace WeatherApp.Web.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Create(TEntity item);
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        void Delete(TEntity item);
        void Update(TEntity item);
    }
}
