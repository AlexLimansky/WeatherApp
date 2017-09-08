using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Web.WeatherServices;
using WeatherApp.Web.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using WeatherApp.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace WeatherApp.Web.Controllers
{
    [Authorize]
    public class WeatherController : Controller
    {
        private IWeatherManager manager;
        private readonly UserManager<ApplicationUser> userManager;
        public WeatherController(IWeatherManager manager, UserManager<ApplicationUser> userManager)
        {
            this.manager = manager;
            this.userManager = userManager;
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
            ModelState.AddModelError("Name", "No city found");
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
