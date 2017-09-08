using WeatherApp.Web.Models;

namespace WeatherApp.Web.WeatherServices
{
    public interface IWeatherService
    {
        CityWeatherInfo GetWeatherInfo(string city);
        void SaveWeatherInfo(CityWeatherInfo info);
    }
}
