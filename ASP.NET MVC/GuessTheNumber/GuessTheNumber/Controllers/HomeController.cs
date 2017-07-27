using System;
using System.Web.Mvc;
using GuessTheNumber.Models;
using GuessTheNumber.Utils;

namespace GuessTheNumber.Controllers
{
    public class HomeController : Controller
    {             
        public ActionResult Index()
        {
            if(HttpContext.Request.Cookies["playerName"] != null)
            {
                Session["Player"] = HttpContext.Request.Cookies["playerName"].Value.ToString();
                Game.Players.Add(new Player(Session["Player"].ToString()));
            }
            if (Session["Player"] != null)
            {                
                ViewBag.PlayerName = Session["Player"];
                if (Game.Number != null)
                {
                    ViewBag.Number = Game.Number;
                    return View("GuessView");
                }
                return View("GameForm");
            }
            return View();
        }
        public ActionResult Login(string name)
        {
            Session["Player"] = name;
            HttpContext.Response.Cookies["playerName"].Value = name;
            Game.Players.Add(new Player(name));
            ViewBag.PlayerName = name;
            if (Game.Number != null)
            {
                ViewBag.Number = Game.Number;
                return View("GuessView");
            }           
            return View("GameForm");
        }

        public ActionResult Logout()
        {
            Player p = Game.Players.Find(a => a.Name == Session["Player"].ToString());
            Session["Player"] = null;
            HttpContext.Response.Cookies["playerName"].Expires = DateTime.Now - TimeSpan.FromDays(1);
            Game.Players.Remove(p);
            return RedirectToAction("Index");
        }

        public ActionResult StartGame(int number)
        {
            ViewBag.PlayerName = Session["Player"];
            if (Game.Number != null)
            {
                return View("GuessView");
            }
            ViewBag.Result = "";
            Game.Number = number;
            Game.Author = ViewBag.PlayerName;
            return View("GuessView");
        }

        public ActionResult Restart()
        {
            Move m = new Move();
            m.playerProposed = Game.Author;
            m.playerGuessed = Session["Player"].ToString();
            m.yourNumber = int.MaxValue;
            m.realNumber = (int)Game.Number;
            m.time = DateTime.Now;
            Game.Log.Add(m);
            Game.Number = null;
            return View("GameForm");
        }

        public ActionResult Guess(int number)
        {
            if(Game.Number == null)
            {
                return View("WinView");
            }
            ViewBag.PlayerName = Session["Player"];
            ViewBag.Result = "";
            int currentNumber = (int)Game.Number;
            Move m = new Move();
            m.playerProposed = Game.Author;
            m.playerGuessed = Session["Player"].ToString();
            m.yourNumber = number;
            m.realNumber = currentNumber;
            m.time = DateTime.Now;
            Game.Log.Add(m);            
            if (currentNumber < number)
            {
                ViewBag.Result = "To much";
            }
            else if (currentNumber > number)
            {
                ViewBag.Result = "To little";
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