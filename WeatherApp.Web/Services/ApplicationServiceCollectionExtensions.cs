using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
                options.Cookies.ApplicationCookie.LoginPath = AppDefaults.LOGIN_DEFAULT_PATH;
                options.Cookies.ApplicationCookie.LogoutPath = AppDefaults.LOGOUT_DEFAULT_PATH;

                options.User.RequireUniqueEmail = true;
            });

            return services;
        }

        public static IServiceCollection AddApplicationLocalizationConfiguration(this IServiceCollection services, string defaultResourcesPath)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>();

                foreach (var item in AppDefaults.CulturesCollection)
                {
                    supportedCultures.Add(new CultureInfo(item));
                }

                options.DefaultRequestCulture = new RequestCulture(AppDefaults.CulturesCollection[0]);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddLocalization(options => options.ResourcesPath = defaultResourcesPath);

            return services;
        }
    }
}