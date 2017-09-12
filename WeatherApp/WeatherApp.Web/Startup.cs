using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeatherApp.Web.Services;
using WeatherApp.Web.Options;
using Microsoft.AspNetCore.Identity;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;
using WeatherApp.Data.Entities;
using WeatherApp.Logic.CoreMVCClesses;
using WeatherApp.Data;

namespace WeatherApp.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var keys = Environment.GetCommandLineArgs().Skip(1).ToArray();
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddCommandLine(keys);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }          

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (Configuration[AppDefaults.DbKey] == AppDefaults.MySqlSelector)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString(AppDefaults.MySqlSelector)));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(AppDefaults.SqlSelector)));
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMemoryCache();

            services.AddApplicationLocalizationConfiguration(AppDefaults.ResourcesDefaultPath);

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddApplicationIdentityConfiguration();

            services.AddApplicationServices();

            services.Configure<WeatherApiOptions>(Configuration.GetSection(nameof(WeatherApiOptions)));
            services.Configure<EmailOptions>(Configuration.GetSection(nameof(EmailOptions)));
            services.Configure<LogOptions>(Configuration.GetSection($"Logging:{nameof(LogOptions)}"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<LogOptions> options)
        {
            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog(NLogConfigurator.Configure(Configuration, env, options));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler(AppDefaults.ErrorDefaultPath);
            }

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseStaticFiles();
            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Weather}/{action=Index}/{id?}");
            });

            DatabaseInitialize(app.ApplicationServices).Wait();
        }
        public async Task DatabaseInitialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                DbInitializer.Initialize(context);
            }

            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach(var role in AppDefaults.RolesCollection)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }          
        }
    }
}
