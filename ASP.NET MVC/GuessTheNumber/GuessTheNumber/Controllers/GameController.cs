using System;
using System.Web.Mvc;
using GuessTheNumber.Models;
using GuessTheNumber.Utils;

namespace GuessTheNumber.Controllers
{
    public class GameController : Controller
    {
        public ActionResult StartGame(int number)
        {
            ViewBag.PlayerName = HttpContext.Request.Cookies["playerName"].Value;
            if (Game.Number != null)
            {
                return View("GuessView");
            }
            Game.Number = number;
            Game.Author = ViewBag.PlayerName;
            return View("GuessView");
        }

        public ActionResult Restart()
        {
            LogEntry newEntry = new LogEntry
            {
                playerProposed = Game.Author,
                playerGuessed = HttpContext.Request.Cookies["playerName"].Value,
                yourNumber = int.MaxValue,
                realNumber = (int)Game.Number,
                time = DateTime.Now
            };
            Game.Log.Add(newEntry);
            Game.Number = null;
            return View("StartView");
        }

        public ActionResult Guess(int number)
        {
            if (Game.Number == null)
            {
                return View("WinView");
            }
            ViewBag.PlayerName = HttpContext.Request.Cookies["playerName"].Value;
            int currentNumber = (int)Game.Number;
            LogEntry newEntry = new LogEntry
            {
                playerProposed = Game.Author,
                playerGuessed = HttpContext.Request.Cookies["playerName"].Value,
                yourNumber = number,
                realNumber = (int)Game.Number,
                time = DateTime.Now
            };
            Game.Log.Add(newEntry);
            if (currentNumber < number)
            {
                ViewBag.Result = Resourses.MoveResultMore;
            }
            else if (currentNumber > number)
            {
                ViewBag.Result = Resourses.MoveResultLess;
            }
            else
            {
                Game.Number = null;
                return View("WinView");
            }
            return View("GuessView");
        }
    }
}