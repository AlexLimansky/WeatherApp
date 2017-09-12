using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Data.Entities;

namespace WeatherApp.Web.ViewComponents
{
    public class CityViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CityWeatherInfo data)
        {
            return this.View(data);
        }
    }
}