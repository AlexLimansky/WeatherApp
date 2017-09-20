using Newtonsoft.Json.Linq;
using WeatherApp.Data.Entities;

namespace WeatherApp.Logic.CoreMVCClesses
{
    public static class WeatherParser
    {
        public static CityWeatherInfo Parse(JObject data)
        {
            var cityResult = data.SelectToken(@"$.name").ToObject<string>();

            var mainTokenResult = data.SelectToken(@"$.main");
            var tempResult = mainTokenResult.SelectToken(@".temp").ToObject<string>();
            var humidityResult = mainTokenResult.SelectToken(@".humidity").ToObject<int>();

            var weatherTokenResult = data.SelectToken(@"$.weather");
            var stateResult = weatherTokenResult[0].SelectToken(@"description").ToObject<string>();
            var iconResult = weatherTokenResult[0].SelectToken(@"icon").ToObject<string>();

            var cloudsResult = data.SelectToken(@"$.clouds.all").ToObject<int>();

            var result = new CityWeatherInfo()
            {
                Name = cityResult,
                Temperature = tempResult,
                Humidity = humidityResult,
                WeatherState = stateResult,
                Icon = iconResult,
                Clouds = cloudsResult
            };

            return result;
        }

        public static void Update(CityWeatherInfo item, CityWeatherInfo result)
        {
            result.Clouds = item.Clouds;
            result.Humidity = item.Humidity;
            result.Icon = item.Icon;
            result.Temperature = item.Temperature;
            result.WeatherState = item.WeatherState;
        }
    }
}