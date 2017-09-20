using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApp.Web.ViewComponents
{
    public class LoginViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = this.HttpContext.User.Identity;
            if (this.HttpContext.User.Identity.Name != null)
            {
                return this.View("Default", user.Name);
            }

            return this.View("NoUser");
        }
    }
}