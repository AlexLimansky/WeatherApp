using System.Web.Mvc;
using GuessTheNumberEF.Logic;

namespace GuessTheNumberEF.MVC.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = GameManager.IsActiveMessage();
            string userName = User.Identity.Name;            
            return View();
        }

        public ActionResult SetNumber(int number, string action)
        {
            string userName = User.Identity.Name;
            if(action == "start")
            {
                return Json(GameManager.StartResult(userName, number));
            }
            return Json(GameManager.GuessResult(userName, number));
        }
    }
}