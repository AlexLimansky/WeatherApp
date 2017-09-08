using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WeatherApp.Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<CityWeatherInfo> Cities { get; set; }
        public ApplicationUser()
        {
            Cities = new List<CityWeatherInfo>();
        }
    }
}
