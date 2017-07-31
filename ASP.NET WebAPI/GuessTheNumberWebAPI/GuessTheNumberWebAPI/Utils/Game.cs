using System.Collections.Generic;
using GuessTheNumberWebAPI.Models;

namespace GuessTheNumberWebAPI.Utils
{
    public static class Game
    {
        public static int? Number { get; set; }
        public static string Author { get; set; }
        public static List<LogEntry> Log = new List<LogEntry>();
    }
}