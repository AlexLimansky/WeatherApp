using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using WeatherApp.Data;
using WeatherApp.Data.Entities;
using WeatherApp.Logic.CoreMVCClesses;
using WeatherApp.Web.Options;
using WeatherApp.Web.Services;

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

            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (this.Configuration[AppDefaults.DB_KEY] == AppDefaults.MY_SQL_SELECTOR)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(this.Configuration.GetConnectionString(AppDefaults.MY_SQL_SELECTOR)));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString(AppDefaults.SQL_SELECTOR)));
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMemoryCache();

            services.AddApplicationLocalizationConfiguration(AppDefaults.RESOURCES_DEFAULT_PATH);

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddApplicationIdentityConfiguration();

            services.AddApplicationServices();

            services.Configure<WeatherApiOptions>(this.Configuration.GetSection(nameof(WeatherApiOptions)));
            services.Configure<EmailOptions>(this.Configuration.GetSection(nameof(EmailOptions)));
            services.Configure<LogOptions>(this.Configuration.GetSection($"Logging:{nameof(LogOptions)}"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<LogOptions> options)
        {
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog(NLogConfigurator.Configure(options));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler(AppDefaults.ERROR_DEFAULT_PATH);
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

            this.DatabaseInitialize(app.ApplicationServices).Wait();
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

            foreach (var role in AppDefaults.RolesCollection)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}