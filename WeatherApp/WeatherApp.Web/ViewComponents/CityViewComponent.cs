using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherApp.Data.Entities;

namespace WeatherApp.Web.ViewComponents
{
    public class CityViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CityWeatherInfo data)
        {
            return View(data);
        }
    }
}
