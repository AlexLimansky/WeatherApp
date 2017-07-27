using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Education_MVC_Routing.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogOn()
        {
            ViewBag.Route = "logon";
            return View("List");
        }

        public ActionResult LogOff()
        {
            ViewBag.Route = "logoff";
            return View("List");
        }
    }
}
