using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GuessTheNumber.Models;

namespace GuessTheNumber.Utils
{
    public static class Game
    {
        public static int? Number { get; set; }
        public static List<Player> Players = new List<Player>();
        public static string Author { get; set; }
        public static List<LogEntry> Log = new List<LogEntry>();
    }
}