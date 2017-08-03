using System.Linq;
using System.Web.Mvc;
using GuessTheNumberEF.Data.Utils;

namespace GuessTheNumberEF.MVC.Controllers
{
    [Authorize]
    public class ArchiveController : Controller
    {
        GameUnitOfWork gm = new GameUnitOfWork();
        public ActionResult Games()
        {
            string userName = User.Identity.Name;
            ViewBag.PlayerName = userName;
            return View(gm.Games.GetAll().Where(g=>g.End != null).OrderByDescending(g=>g.End));
        }

        public ActionResult GameDetails(int gameId)
        {
            string userName = User.Identity.Name;
            ViewBag.PlayerName = userName;
            return View(gm.Games.Get(gameId));
        }
    }
}