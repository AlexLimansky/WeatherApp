using System.Web.Mvc;
using GuessTheNumberEF.Logic;

namespace GuessTheNumberEF.MVC.Controllers
{
    [Authorize]
    public class ArchiveController : Controller
    {
        private IGameManager gameManager;
        private IAuthManager authManager;
        public ArchiveController(IGameManager gm, IAuthManager am)
        {
            gameManager = gm;
            authManager = am;
        }

        public ActionResult Games()
        {
            ViewBag.PlayerName = authManager.GetUserName();
            return View(gameManager.GetAllGames());
        }

        public ActionResult GameDetails(int gameId)
        {
            ViewBag.PlayerName = authManager.GetUserName();
            return View(gameManager.GetOneGame(gameId));
        }
    }
}