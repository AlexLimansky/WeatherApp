using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WeatherApp.Data.Entities;
using WeatherApp.Logic.Interfaces;

namespace WeatherApp.Web.Controllers
{
    [Authorize]
    public class WeatherController : Controller
    {
        private IWeatherManager manager;
        private UserManager<ApplicationUser> userManager;
        private IStringLocalizer<WeatherController> localizer;

        public WeatherController(IWeatherManager manager, UserManager<ApplicationUser> userManager, IStringLocalizer<WeatherController> localizer)
        {
            this.manager = manager;
            this.userManager = userManager;
            this.localizer = localizer;
        }

        public IActionResult Index()
        {
            var userId = this.userManager.GetUserId(this.HttpContext.User);
            return this.View(this.manager.GetWeatherInfoCollection(userId));
        }

        [HttpGet]
        public IActionResult AddCity()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult AddCity(CityWeatherInfo cityname)
        {
            var userId = this.userManager.GetUserId(this.HttpContext.User);

            if (this.manager.AddCity(userId, cityname.Name))
            {
                return this.RedirectToAction((nameof(WeatherController.Index)));
            }

            this.ModelState.AddModelError("Name", this.localizer["No city found"]);
            return this.View(cityname);
        }

        [HttpPost]
        public IActionResult RemoveCity([FromForm]string city)
        {
            var userId = this.userManager.GetUserId(this.HttpContext.User);
            this.manager.RemoveCity(userId, city);
            return this.RedirectToAction((nameof(WeatherController.Index)));
        }

        public IActionResult Error()
        {
            var ex = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
            return this.View(ex);
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            this.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return this.LocalRedirect(returnUrl);
        }
    }
}