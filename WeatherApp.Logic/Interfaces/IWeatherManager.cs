using System.Collections.Generic;
using WeatherApp.Data.Entities;

namespace WeatherApp.Logic.Interfaces
{
    public interface IWeatherManager
    {
        IEnumerable<CityWeatherInfo> GetWeatherInfoCollection(string username);

        bool AddCity(string username, string city);

        void RemoveCity(string username, string city);
    }
}