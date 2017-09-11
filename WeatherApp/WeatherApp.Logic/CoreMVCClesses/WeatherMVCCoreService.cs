using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
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
            logger = loggerFactory.CreateLogger<WeatherMVCCoreService>();
        }

        public CityWeatherInfo GetWeatherInfo(string city)
        {
            logger.LogTrace($"TRACE - {DateTime.Now} - Entered GetWeatherInfo method");
            if (!cache.TryGetValue(city, out CityWeatherInfo result))
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(options.baseurl + $"{city}&units={options.units}&appid={options.appid}")
                };
                var response = client.GetAsync(client.BaseAddress).Result;
                response.EnsureSuccessStatusCode();
                var content = response.Content.ReadAsStringAsync().Result;
                var tempResult = JObject.Parse(content).SelectToken(@"$.main.temp").ToObject<string>();
                var cityResult = JObject.Parse(content).SelectToken(@"$.name").ToObject<string>();
                if (String.Equals(cityResult, city, StringComparison.OrdinalIgnoreCase))
                {
                    logger.LogDebug($"DEBUG - {DateTime.Now} - Received info about {city} from WeatherAPI");
                    result = new CityWeatherInfo() { Name = city, Temperature = tempResult };
                    SaveWeatherInfo(result);
                    logger.LogTrace($"TRACE - {DateTime.Now} - Ended GetWeatherInfo method");
                    return result;
                }
                logger.LogDebug($"DEBUG - {DateTime.Now} - No info about {city} was found with WeatherAPI");
                logger.LogTrace($"TRACE - {DateTime.Now} - Ended GetWeatherInfo method");
                return null;
            }
            logger.LogDebug($"DEBUG - {DateTime.Now} - Returned info about {city} from cache");
            logger.LogTrace($"TRACE - {DateTime.Now} - Ended GetWeatherInfo method");
            return result;
        }
        public void SaveWeatherInfo(CityWeatherInfo info)
        {
            logger.LogTrace($"TRACE - {DateTime.Now} - Entered SaveWeatherInfo method");
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
            cache.Set(info.Name, info, cacheEntryOptions);
            logger.LogDebug($"DEBUG - {DateTime.Now} - Added info about {info.Name} to cache");
            logger.LogTrace($"TRACE - {DateTime.Now} - Ended SaveWeatherInfo method");
        }
    }
}
