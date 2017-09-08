using System.Collections.Generic;
using WeatherApp.Web.Data;
using WeatherApp.Web.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace WeatherApp.Web.WeatherServices
{
    public class WeatherManager : IWeatherManager
    {
        private readonly ApplicationDbContext context;
        private readonly IWeatherService service;
        private readonly ILogger logger;
        public WeatherManager(ApplicationDbContext context, IWeatherService service, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.service = service;
            logger = loggerFactory.CreateLogger<WeatherManager>();
        }
        public IEnumerable<CityWeatherInfo> WeatherInfoCollection(string userId)
        {
            logger.LogTrace($"TRACE - {DateTime.Now} - Entered WeatherInfoCollection method");
            var user = context.Users.Include(c => c.CityWeatherInfos).First(u => u.Id == userId);
            logger.LogDebug($"DEBUG - {DateTime.Now} - Get {user.UserName}'s collection of forecasts");
            var result = new List<CityWeatherInfo>();
            foreach(var entry in user.CityWeatherInfos)
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
        private bool CityExists(string city)
        {
            var item = context.Cities.First(c => c.Name == city);
            return item != null;
        }
        public bool AddCity(string userId, string city)
        {
            logger.LogTrace($"TRACE - {DateTime.Now} - Entered AddCity method");
            var info = service.GetWeatherInfo(city);           
            if(info != null)
            {
                var user = context.Users.Include(c => c.CityWeatherInfos).First(u => u.Id == userId);
                if (CityExists(city))
                {
                    var item = context.Cities.First(c => c.Name == city);
                    item.Temperature = info.Temperature;
                    context.Cities.Update(item);
                    user.CityWeatherInfos.Add(item);
                    context.SaveChanges();
                    return true;
                }                
                if (user.CityWeatherInfos == null)
                {
                    logger.LogDebug($"DEBUG - {DateTime.Now} - Added first info to {user.UserName}'s collection");
                    user.CityWeatherInfos = new List<CityWeatherInfo>();
                    user.CityWeatherInfos.Add(info);
                    context.Users.Update(user);
                }
                else
                {
                    if(user.CityWeatherInfos.Any(c => c.Name == info.Name))
                    {
                        logger.LogDebug($"DEBUG - {DateTime.Now} - Edited info in {user.UserName}'s collection");
                        var item = user.CityWeatherInfos.First(c => c.Name == info.Name);
                        item.Temperature = info.Temperature;
                        context.Cities.Update(item);
                    }
                    else
                    {
                        logger.LogDebug($"DEBUG - {DateTime.Now} - Added new info to {user.UserName}'s collection");
                        user.CityWeatherInfos.Add(info);
                        context.Users.Update(user);
                    }
                }               
                context.SaveChanges();
                logger.LogInformation($"INFO - {DateTime.Now} - Added info to {user.UserName}'s collection");
                logger.LogTrace($"TRACE - {DateTime.Now} - Ended AddCity method");
                return true;
            }
            return false;
        }
        public void RemoveCity(string userId, string city)
        {
            logger.LogTrace($"TRACE - {DateTime.Now} - Entered RemoveCity method");
            var user = context.Users.Include(c => c.CityWeatherInfos).First(u => u.Id == userId);
            var item = user.CityWeatherInfos.First(c => c.Name == city);
            user.CityWeatherInfos.Remove(item);
            context.SaveChanges();
            logger.LogInformation($"INFO - {DateTime.Now} - Removed info from {user.UserName}'s collection");
            logger.LogTrace($"TRACE - {DateTime.Now} - Ended RemoveCity method");
        }
    }
}
