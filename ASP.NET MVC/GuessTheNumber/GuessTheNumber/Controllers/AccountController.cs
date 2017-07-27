using System;
using System.Web.Mvc;
using GuessTheNumber.Utils;
using GuessTheNumber.Models;

namespace GuessTheNumber.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index", "Home", new { alert = Resourses.LoginBlankEntryAlert });
            }
            if (Game.Players.Exists(a=>a.Name == name))
            {
                return RedirectToAction("Index", "Home", new { alert = Resourses.LoginSameEntryAlert });
            }
            HttpContext.Response.Cookies["playerName"].Value = name;
            Game.Players.Add(new Player(name));
            ViewBag.PlayerName = name;
            if (Game.Number != null)
            {
                ViewBag.Number = Game.Number;
                return View(Resourses.PathToGuessView);
            }
            return View(Resourses.PathToStartView);
        }

        public ActionResult Logout()
        {
            Player p = Game.Players.Find(a => a.Name == HttpContext.Request.Cookies["playerName"].Value.ToString());
            HttpContext.Response.Cookies["playerName"].Expires = DateTime.Now - TimeSpan.FromDays(1);
            Game.Players.Remove(p);
            return RedirectToAction("Index", "Home");
        }
    }
}