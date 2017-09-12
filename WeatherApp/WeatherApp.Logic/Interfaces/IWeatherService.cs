using WeatherApp.Data.Entities;

namespace WeatherApp.Logic.Interfaces
{
    public interface IWeatherService
    {
        CityWeatherInfo GetWeatherInfo(string city);

        void SaveWeatherInfo(CityWeatherInfo info);
    }
}