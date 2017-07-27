using System.Web.Mvc;
using GuessTheNumber.Models;
using GuessTheNumber.Utils;

namespace GuessTheNumber.Controllers
{
    public class HomeController : Controller
    {             
        public ActionResult Index(bool? isRestart, string alert)
        {
            if(alert != null)
            {
                ViewBag.Alert = alert;
                return View();
            }
            string currentPlayer = string.Empty;
            if(HttpContext.Request.Cookies["playerName"] != null)
            {
                currentPlayer = HttpContext.Request.Cookies["playerName"].Value;
                if(isRestart == null)
                {
                    Game.Players.Add(new Player(currentPlayer));
                }                
            }
            if (currentPlayer != string.Empty)
            {                
                ViewBag.PlayerName = currentPlayer;
                if (Game.Number != null)
                {
                    ViewBag.Number = Game.Number;
                    return View(Resourses.PathToGuessView);
                }
                return View(Resourses.PathToStartView);
            }
            return View();
        }              
    }
}