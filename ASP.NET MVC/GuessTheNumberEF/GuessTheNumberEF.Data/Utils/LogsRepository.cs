using System.Collections.Generic;
using System.Data.Entity;
using GuessTheNumberEF.Data.Entities;
using GuessTheNumberEF.Data.Contexts;

namespace GuessTheNumberEF.Data.Utils
{
    public class LogsRepository : IRepository<LogEntry>
    {
        private DataContext db;

        public LogsRepository(DataContext context)
        {
            db = context;
        }

        public IEnumerable<LogEntry> GetAll()
        {
            return db.Logs;
        }

        public LogEntry Get(int id)
        {
            return db.Logs.Find(id);
        }

        public void Create(LogEntry item)
        {
            db.Logs.Add(item);
        }

        public void Update(LogEntry item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            LogEntry item = db.Logs.Find(id);
            if (item != null)
                db.Logs.Remove(item);
        }
    }
}
