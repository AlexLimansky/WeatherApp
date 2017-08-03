using System;

namespace GuessTheNumberEF.Data.Entities
{
    public class LogEntry
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string PlayerName { get; set; }
        public DateTime Time { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }       
    }
}
