using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using WeatherApp.Data;
using WeatherApp.Data.Repository;
using WeatherApp.Logic.CoreMVCClesses;
using WeatherApp.Logic.Interfaces;

namespace WeatherApp.Web.Services
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, AuthMessageSender>();

            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IWeatherService, WeatherMVCCoreService>();
            services.AddScoped<IWeatherManager, WeatherMVCCoreManager>();

            services.AddScoped<DbContext, ApplicationDbContext>();

            return services;
        }

        public static IServiceCollection AddApplicationIdentityConfiguration(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
                options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOut";

                options.User.RequireUniqueEmail = true;
            });

            return services;
        }

        public static IServiceCollection AddApplicationLocalizationConfiguration(this IServiceCollection services, string defaultResourcesPath)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddLocalization(options => options.ResourcesPath = defaultResourcesPath);

            return services;
        }
    }
}
