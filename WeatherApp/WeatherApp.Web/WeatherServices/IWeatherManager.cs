using System.Collections.Generic;
using WeatherApp.Web.Models;

namespace WeatherApp.Web.WeatherServices
{
    public interface IWeatherManager
    {
        IEnumerable<CityWeatherInfo> WeatherInfoCollection(string username);
        bool AddCity(string username, string city);
        void RemoveCity(string username, string city);
    }
}
