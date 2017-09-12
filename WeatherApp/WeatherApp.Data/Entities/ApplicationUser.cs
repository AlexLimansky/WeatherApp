using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WeatherApp.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<CityWeatherInfo> CityWeatherInfos { get; set; }
    }
}