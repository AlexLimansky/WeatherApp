using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Education_JavaScript.Infrastructure;

namespace Education_JavaScript.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult GetData(bool withError)
        {
            if (withError)
            {
                throw new Exception();
            }
            return Json(new
                        {
                            Id = 1,
                            Name = "Ivan Popov",
                            Age = 35,
                            Sex = Sex.Male
                        }, 
                        JsonRequestBehavior.AllowGet);
        }
    }
}
