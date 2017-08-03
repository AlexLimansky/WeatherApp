using System.Web.Mvc;
using GuessTheNumberEF.Logic;

namespace GuessTheNumberEF.MVC.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private IGameManager manager;

        public GameController(IGameManager gm)
        {
            manager = gm;
        }

        public ActionResult Index()
        {
            ViewBag.Message = manager.IsActiveMessage();
            string userName = User.Identity.Name;            
            return View();
        }

        public ActionResult SetNumber(int number, string action)
        {
            string userName = User.Identity.Name;
            if(action == "start")
            {
                return Json(manager.StartResult(userName, number));
            }
            return Json(manager.GuessResult(userName, number));
        }
    }
}