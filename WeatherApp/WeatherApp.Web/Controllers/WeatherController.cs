using Microsoft.AspNetCore.Mvc;
using WeatherApp.Web.WeatherServices;
using Microsoft.AspNetCore.Identity;
using WeatherApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace WeatherApp.Web.Controllers
{
    [Authorize]
    public class WeatherController : Controller
    {
        private IWeatherManager manager;
        private readonly UserManager<ApplicationUser> userManager;
        private IStringLocalizer<WeatherController> localizer;
        public WeatherController(IWeatherManager manager, UserManager<ApplicationUser> userManager, IStringLocalizer<WeatherController> localizer)
        {
            this.manager = manager;
            this.userManager = userManager;
            this.localizer = localizer;
        }
        public IActionResult Index()
        {
            var userId = userManager.GetUserId(HttpContext.User);
            return View(manager.WeatherInfoCollection(userId));
        }
        [HttpGet]
        public IActionResult AddCity()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCity(CityWeatherInfo cityname)
        {
            var userId = userManager.GetUserId(HttpContext.User);
            if (manager.AddCity(userId, cityname.Name))
            {
                return RedirectToAction((nameof(WeatherController.Index)));
            }
            ModelState.AddModelError("Name", localizer["No city found"]);
            return View(cityname);
        }
        [HttpPost]
        public IActionResult RemoveCity([FromForm]string city)
        {
            var userId = userManager.GetUserId(HttpContext.User);
            manager.RemoveCity(userId, city);
            return RedirectToAction((nameof(WeatherController.Index)));
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
