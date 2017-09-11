using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherApp.Data.Entities;
using WeatherApp.Data.Repository;
using WeatherApp.Logic.Interfaces;

namespace WeatherApp.Logic.CoreMVCClesses
{
    public class WeatherMVCCoreManager : IWeatherManager
    {
        private IRepository<ApplicationUser> users;
        private IRepository<CityWeatherInfo> cities;
        private readonly IWeatherService service;
        private readonly ILogger logger;
        public WeatherMVCCoreManager(IRepository<ApplicationUser> users, IRepository<CityWeatherInfo> cities, IWeatherService service, ILoggerFactory loggerFactory)
        {
            this.users = users;
            this.cities = cities;
            this.service = service;
            logger = loggerFactory.CreateLogger<WeatherMVCCoreManager>();
        }
        public IEnumerable<CityWeatherInfo> WeatherInfoCollection(string userId)
        {
            logger.LogTrace($"TRACE - {DateTime.Now} - Entered WeatherInfoCollection method");
            var user = users.Get(u => u.Id == userId, c => c.CityWeatherInfos);
            logger.LogDebug($"DEBUG - {DateTime.Now} - Get {user.UserName}'s collection of forecasts");
            var result = new List<CityWeatherInfo>();
            foreach (var entry in user.CityWeatherInfos)
            {
                var info = service.GetWeatherInfo(entry.Name);
                if (info != null)
                {
                    result.Add(info);
                }
            }
            logger.LogTrace($"TRACE - {DateTime.Now} - Ended WeatherInfoCollection method");
            return result;
        }
        private bool CheckCity(string city)
        {
            logger.LogTrace($"TRACE - {DateTime.Now} - Entered CheckCity method");
            var info = service.GetWeatherInfo(city);
            logger.LogTrace($"TRACE - {DateTime.Now} - Ended CheckCity method");
            return info != null;
        }
        public bool AddCity(string userId, string city)
        {
            logger.LogTrace($"TRACE - {DateTime.Now} - Entered AddCity method");
            var info = service.GetWeatherInfo(city);
            if (info != null)
            {
                var user = users.Get(u => u.Id == userId, c => c.CityWeatherInfos);
                if (cities.GetAll().Any(c => c.Name == city))
                {
                    var item = cities.Get(city);
                    item.Temperature = info.Temperature;
                    user.CityWeatherInfos.Add(item);
                    users.Update(user);
                    return true;
                }
                if (user.CityWeatherInfos == null)
                {
                    logger.LogDebug($"DEBUG - {DateTime.Now} - Added first info to {user.UserName}'s collection");
                    user.CityWeatherInfos = new List<CityWeatherInfo>();
                    user.CityWeatherInfos.Add(info);
                    users.Update(user);
                }
                else
                {
                    if (user.CityWeatherInfos.Any(c => c.Name == info.Name))
                    {
                        logger.LogDebug($"DEBUG - {DateTime.Now} - Edited info in {user.UserName}'s collection");
                        var item = user.CityWeatherInfos.First(c => c.Name == info.Name);
                        item.Temperature = info.Temperature;
                        cities.Update(item);
                    }
                    else
                    {
                        logger.LogDebug($"DEBUG - {DateTime.Now} - Added new info to {user.UserName}'s collection");
                        user.CityWeatherInfos.Add(info);
                        users.Update(user);
                    }
                }
                logger.LogInformation($"INFO - {DateTime.Now} - Added info to {user.UserName}'s collection");
                logger.LogTrace($"TRACE - {DateTime.Now} - Ended AddCity method");
                return true;
            }
            return false;
        }
        public void RemoveCity(string userId, string city)
        {
            logger.LogTrace($"TRACE - {DateTime.Now} - Entered RemoveCity method");
            var user = users.Get(u => u.Id == userId, c => c.CityWeatherInfos);
            var item = cities.Get(city);
            user.CityWeatherInfos.Remove(item);
            users.Update(user);
            logger.LogInformation($"INFO - {DateTime.Now} - Removed info from {user.UserName}'s collection");
            logger.LogTrace($"TRACE - {DateTime.Now} - Ended RemoveCity method");
        }
    }
}
