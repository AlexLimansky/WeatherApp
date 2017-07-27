using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Education_MVC_Routing
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "category/{categoryName}",
                url: "category/{categoryName}",
                defaults: new { controller = "Categories", action = "Category", categoryName = UrlParameter.Optional });

            routes.MapRoute(
                name: "category/{categoryName}/subcategories",
                url: "category/{categoryName}/subcategories/",
                defaults: new { controller = "Categories", action = "SubCategories", categoryName = UrlParameter.Optional });

            routes.MapRoute(
                name: "Category/{categoryName}/{subCategoryName}",
                url: "Category/{categoryName}/{subCategoryName}",
                defaults: new { controller = "Categories", action = "SubCategory", categoryName = UrlParameter.Optional, subCategoryName = UrlParameter.Optional });
            

            routes.MapRoute(
                name: "Categories",
                url: "Categories/",
                defaults: new { controller = "Categories", action = "List" });

            routes.MapRoute(
                name: "logon",
                url: "logon/",
                defaults: new { controller = "Account", action = "LogOn" });

            routes.MapRoute(
                name: "logoff",
                url: "logoff/",
                defaults: new { controller = "Account", action = "LogOff" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );         
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}