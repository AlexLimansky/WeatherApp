using System;
using System.Collections.Generic;

namespace GuessTheNumberEF.Data.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string AuthorName { get; set; }
        public string WinnerName { get; set; }        
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public int PlayersCount { get; set; }
        public virtual ICollection<LogEntry> Log { get; set; }
    }
}
