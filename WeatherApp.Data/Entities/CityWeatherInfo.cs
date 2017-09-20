using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Data.Entities
{
    public class CityWeatherInfo
    {
        [Key]
        public string Name { get; set; }

        public string Temperature { get; set; }

        public string WeatherState { get; set; }

        public int Humidity { get; set; }

        public int Clouds { get; set; }

        public string Icon { get; set; }
    }
}