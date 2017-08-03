using System.Web.Mvc;
using GuessTheNumberEF.Logic;

namespace GuessTheNumberEF.MVC.Controllers
{
    //[Authorize]
    public class GameController : Controller
    {
        private IGameManager gameManager;
        private IAuthManager authManager;
        public GameController(IGameManager gm, IAuthManager am)
        {
            gameManager = gm;
            authManager = am;
        }

        public ActionResult Index()
        {
            ViewBag.Message = gameManager.IsActiveMessage();            
            return View();
        }

        public ActionResult SetNumber(int number, string action)
        {
            string userName = authManager.GetUserName();
            if(action == "start")
            {
                return Json(gameManager.StartResult(userName, number));
            }
            return Json(gameManager.GuessResult(userName, number));
        }
    }
}