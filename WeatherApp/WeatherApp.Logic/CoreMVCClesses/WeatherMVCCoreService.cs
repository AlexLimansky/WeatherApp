using System;
using System.Globalization;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using WeatherApp.Data.Entities;
using WeatherApp.Logic.Interfaces;

namespace WeatherApp.Logic.CoreMVCClesses
{
    public class WeatherMVCCoreService : IWeatherService
    {
        private IMemoryCache cache;
        private WeatherApiOptions options;
        private ILogger logger;

        public WeatherMVCCoreService(IMemoryCache cache, IOptions<WeatherApiOptions> options, ILoggerFactory loggerFactory)
        {
            this.cache = cache;
            this.options = options.Value;
            this.logger = loggerFactory.CreateLogger<WeatherMVCCoreService>();
        }

        public CityWeatherInfo GetWeatherInfo(string city)
        {
            var lang = CultureInfo.CurrentCulture.Name;
            this.logger.LogTrace($"TRACE - {DateTime.Now} - Entered GetWeatherInfo method");

            if (!this.cache.TryGetValue(city + lang, out CityWeatherInfo result))
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(this.options.Baseurl + $"{city}&units={this.options.Units}&lang={lang}&appid={this.options.Appid}"),
                    Timeout = TimeSpan.FromSeconds(5)
                };
                var response = client.GetAsync(client.BaseAddress).Result;
                response.EnsureSuccessStatusCode();
                var content = response.Content.ReadAsStringAsync().Result;
                var weather = WeatherParser.Parse(JObject.Parse(content));
                if (string.Equals(weather.Name, city, StringComparison.OrdinalIgnoreCase))
                {
                    this.logger.LogDebug($"DEBUG - {DateTime.Now} - Received info about {city} from WeatherAPI");

                    result = weather;
                    this.SaveWeatherInfo(result);

                    this.logger.LogTrace($"TRACE - {DateTime.Now} - Ended GetWeatherInfo method");
                    return result;
                }

                this.logger.LogDebug($"DEBUG - {DateTime.Now} - No info about {city} was found with WeatherAPI");
                this.logger.LogTrace($"TRACE - {DateTime.Now} - Ended GetWeatherInfo method");
                return null;
            }

            this.logger.LogDebug($"DEBUG - {DateTime.Now} - Returned info about {city} from cache");
            this.logger.LogTrace($"TRACE - {DateTime.Now} - Ended GetWeatherInfo method");
            return result;
        }

        public void SaveWeatherInfo(CityWeatherInfo info)
        {
            var lang = CultureInfo.CurrentCulture.Name;

            this.logger.LogTrace($"TRACE - {DateTime.Now} - Entered SaveWeatherInfo method");

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
            this.cache.Set(info.Name + lang, info, cacheEntryOptions);

            this.logger.LogDebug($"DEBUG - {DateTime.Now} - Added info about {info.Name} to cache");
            this.logger.LogTrace($"TRACE - {DateTime.Now} - Ended SaveWeatherInfo method");
        }
    }
}