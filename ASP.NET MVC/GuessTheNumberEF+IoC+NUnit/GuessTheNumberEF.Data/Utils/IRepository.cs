using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace GuessTheNumberEF.Data.Utils
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
