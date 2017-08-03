using System.Data.Entity;
using GuessTheNumberEF.Data.Entities;

namespace GuessTheNumberEF.Data.Contexts
{
    public class DataContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<LogEntry> Logs { get; set; }

        public DataContext() : base("DefaultConnection")
        {

        }

    }
}
