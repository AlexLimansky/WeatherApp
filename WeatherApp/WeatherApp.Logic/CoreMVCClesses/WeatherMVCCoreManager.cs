using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WeatherApp.Data.Entities;
using WeatherApp.Data.Repository;
using WeatherApp.Logic.Interfaces;

namespace WeatherApp.Logic.CoreMVCClesses
{
    public class WeatherMVCCoreManager : IWeatherManager
    {
        private IRepository<ApplicationUser> users;
        private IRepository<CityWeatherInfo> cities;
        private IWeatherService service;
        private ILogger logger;

#pragma warning disable Nix05 // Too many parameters in constructor
        public WeatherMVCCoreManager(
            IRepository<ApplicationUser> users,
            IRepository<CityWeatherInfo> cities,
            IWeatherService service,
            ILoggerFactory loggerFactory)
#pragma warning restore Nix05 // Too many parameters in constructor
        {
            this.users = users;
            this.cities = cities;
            this.service = service;
            this.logger = loggerFactory.CreateLogger<WeatherMVCCoreManager>();
        }

        public IEnumerable<CityWeatherInfo> WeatherInfoCollection(string userId)
        {
            var user = this.users.Get(u => u.Id == userId, c => c.CityWeatherInfos);
            var result = new List<CityWeatherInfo>();

            this.logger.LogTrace($"TRACE - {DateTime.Now} - Entered WeatherInfoCollection method");
            this.logger.LogDebug($"DEBUG - {DateTime.Now} - Get {user.UserName}'s collection of forecasts");

            foreach (var entry in user.CityWeatherInfos)
            {
                var info = this.service.GetWeatherInfo(entry.Name);

                if (info != null)
                {
                    result.Add(info);
                }
            }

            this.logger.LogTrace($"TRACE - {DateTime.Now} - Ended WeatherInfoCollection method");
            return result;
        }

        public bool AddCity(string userId, string city)
        {
            this.logger.LogTrace($"TRACE - {DateTime.Now} - Entered AddCity method");
            var info = this.service.GetWeatherInfo(city);

            if (info != null)
            {
                var user = this.users.Get(u => u.Id == userId, c => c.CityWeatherInfos);

                if (this.cities.GetAll().Any(c => c.Name == city))
                {
                    var item = this.cities.Get(city);
                    item = info;
                    item.Name = city;
                    user.CityWeatherInfos.Add(item);
                    this.users.Update(user);
                    return true;
                }

                if (user.CityWeatherInfos == null)
                {
                    this.logger.LogDebug($"DEBUG - {DateTime.Now} - Added first info to {user.UserName}'s collection");
                    user.CityWeatherInfos = new List<CityWeatherInfo>();
                    user.CityWeatherInfos.Add(info);
                    this.users.Update(user);
                }
                else
                {
                    if (user.CityWeatherInfos.Any(c => c.Name == info.Name))
                    {
                        this.logger.LogDebug($"DEBUG - {DateTime.Now} - Edited info in {user.UserName}'s collection");
                        var item = user.CityWeatherInfos.First(c => c.Name == info.Name);
                        item.Temperature = info.Temperature;
                        this.cities.Update(item);
                    }
                    else
                    {
                        this.logger.LogDebug($"DEBUG - {DateTime.Now} - Added new info to {user.UserName}'s collection");
                        user.CityWeatherInfos.Add(info);
                        this.users.Update(user);
                    }
                }

                this.logger.LogInformation($"INFO - {DateTime.Now} - Added info to {user.UserName}'s collection");
                this.logger.LogTrace($"TRACE - {DateTime.Now} - Ended AddCity method");
                return true;
            }

            return false;
        }

        public void RemoveCity(string userId, string city)
        {
            this.logger.LogTrace($"TRACE - {DateTime.Now} - Entered RemoveCity method");

            var user = this.users.Get(u => u.Id == userId, c => c.CityWeatherInfos);
            var item = this.cities.Get(city);

            user.CityWeatherInfos.Remove(item);
            this.users.Update(user);

            this.logger.LogInformation($"INFO - {DateTime.Now} - Removed info from {user.UserName}'s collection");
            this.logger.LogTrace($"TRACE - {DateTime.Now} - Ended RemoveCity method");
        }
    }
}