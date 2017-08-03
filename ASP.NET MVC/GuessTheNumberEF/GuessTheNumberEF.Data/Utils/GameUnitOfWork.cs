using System;
using GuessTheNumberEF.Data.Contexts;

namespace GuessTheNumberEF.Data.Utils
{
    public class GameUnitOfWork : IDisposable
    {
        private DataContext db = new DataContext();
        private LogsRepository logsRepository;
        private GameRepository gameRepository;

        public LogsRepository Logs
        {
            get
            {
                if (logsRepository == null)
                    logsRepository = new LogsRepository(db);
                return logsRepository;
            }
        }

        public GameRepository Games
        {
            get
            {
                if (gameRepository == null)
                    gameRepository = new GameRepository(db);
                return gameRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
