using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WeatherApp.Web.ViewComponents
{
    public class LoginViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = HttpContext.User.Identity;
            if (HttpContext.User.Identity.Name != null)
            {
                return View("Default", user.Name);
            }
            return View("NoUser");
        }
    }
}
