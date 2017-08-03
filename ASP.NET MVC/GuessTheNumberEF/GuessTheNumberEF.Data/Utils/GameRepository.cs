using System.Collections.Generic;
using System.Data.Entity;
using GuessTheNumberEF.Data.Entities;
using GuessTheNumberEF.Data.Contexts;

namespace GuessTheNumberEF.Data.Utils
{
    public class GameRepository : IRepository<Game>
    {
        private DataContext db;

        public GameRepository(DataContext context)
        {
            db = context;
        }

        public IEnumerable<Game> GetAll()
        {
            return db.Games;
        }

        public Game Get(int id)
        {
            return db.Games.Find(id);
        }

        public Game GetCurrent()
        {
            return db.Games.FirstOrDefaultAsync(g => g.End == null).Result; 
        }

        public bool UserInGame(string userName, int gameId)
        {
            bool isAuthor = db.Games.AnyAsync(g => g.AuthorName == userName && g.Id == gameId).Result;
            return db.Logs.AnyAsync(l => l.GameId == gameId && l.PlayerName == userName).Result || isAuthor;
        }

        public void Create(Game item)
        {
            db.Games.Add(item);
        }

        public void Update(Game item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Game item = db.Games.Find(id);
            if (item != null)
                db.Games.Remove(item);
        }
    }
}
