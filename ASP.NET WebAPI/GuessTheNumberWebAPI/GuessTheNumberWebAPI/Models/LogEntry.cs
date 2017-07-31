using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuessTheNumberWebAPI.Models
{
    public class LogEntry
    {
        public DateTime time { get; set; }
        public string playerGuessed { get; set; }
        public string playerProposed { get; set; }
        public int yourNumber { get; set; }
        public int realNumber { get; set; }
    }
}