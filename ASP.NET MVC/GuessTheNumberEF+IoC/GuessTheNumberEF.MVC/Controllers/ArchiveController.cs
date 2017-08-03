using System.Web.Mvc;
using GuessTheNumberEF.Logic;

namespace GuessTheNumberEF.MVC.Controllers
{
    [Authorize]
    public class ArchiveController : Controller
    {
        private IGameManager manager;
        public ArchiveController(IGameManager gm)
        {
            manager = gm;
        }

        public ActionResult Games()
        {
            string userName = User.Identity.Name;
            ViewBag.PlayerName = userName;
            return View(manager.GetAllGames());
        }

        public ActionResult GameDetails(int gameId)
        {
            string userName = User.Identity.Name;
            ViewBag.PlayerName = userName;
            return View(manager.GetOneGame(gameId));
        }
    }
}