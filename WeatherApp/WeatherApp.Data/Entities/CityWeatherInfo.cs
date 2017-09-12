using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Data.Entities
{
    public class CityWeatherInfo
    {
        [Key]
        public string Name { get; set; }

        public string Temperature { get; set; }
    }
}