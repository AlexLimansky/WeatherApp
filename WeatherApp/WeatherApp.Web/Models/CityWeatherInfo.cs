using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Web.Models
{
    public class CityWeatherInfo
    {
        [Key]
        public string Name { get; set; }
        public string Temperature { get; set; }
    }
}
