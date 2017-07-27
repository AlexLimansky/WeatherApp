using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuessTheNumber.Models
{
    public class Player
    {
        public string Name { get; set; }
        public Player(string name)
        {
            Name = name;
        }
    }
}