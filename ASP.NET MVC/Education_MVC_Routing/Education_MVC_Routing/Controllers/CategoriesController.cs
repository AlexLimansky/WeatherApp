using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Education_MVC_Routing.Controllers
{
    public class CategoriesController : Controller
    {
        public ActionResult List()
        {
            ViewBag.Route = "/Categories";
            return View("List");
        }

        public ActionResult Category(string categoryName)
        {
            ViewBag.Route = "category/"+ categoryName;
            return View("List");
        }

        public ActionResult SubCategories(string categoryName)
        {
            ViewBag.Route = "category/"+ categoryName + "/subcategories";
            return View("List");
        }

        public ActionResult SubCategory(string categoryName, string subCategoryName)
        {
            ViewBag.Route = "category/" + categoryName + "/" + subCategoryName;
            return View("List");
        }
    }
}
