using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeatherApp.Web.Data;
using WeatherApp.Web.Models;
using WeatherApp.Web.Services;
using WeatherApp.Web.Options;
using Microsoft.AspNetCore.Identity;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Options;
using FluentValidation.AspNetCore;
using WeatherApp.Web.Validators;
using WeatherApp.Web.WeatherServices;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using FluentValidation;
using WeatherApp.Web.Models.AccountViewModels;

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
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }          
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<Startup>>();
            logger.LogInformation($"INFO - {DateTime.Now} - App started with MySQL database");
            if (Configuration["db"] == "mysql")
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("MySQLConnection")));
                logger.LogInformation($"INFO - {DateTime.Now} - App started with MySQL database");
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                logger.LogInformation($"INFO - {DateTime.Now} - App started with MSSQL database");
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMemoryCache();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ApplicationUserValidator>())
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // Cookie settings
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
                options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOut";

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IValidator<RegisterViewModel>, ApplicationUserValidator>();

            services.AddScoped<IRepository<ApplicationUser>, GenericRepository<ApplicationUser>>();
            services.AddScoped<IRepository<CityWeatherInfo>, GenericRepository<CityWeatherInfo>>();
            services.AddSingleton<IWeatherService, WeatherService>();
            services.AddScoped<IWeatherManager, WeatherManager>();
            services.AddScoped<DbContext, ApplicationDbContext>();

            services.Configure<WeatherApiOptions>(Configuration.GetSection("WeatherApi"));
            services.Configure<EmailOptions>(Configuration.GetSection("Email"));
            services.Configure<LogOptions>(Configuration.GetSection("Logging:LogLevel"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<LogOptions> options)
        {
            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog(NLogConfigurator.Configure(Configuration, env, options));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Weather/Error");
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ru")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseStaticFiles();
            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

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
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
        }
    }
}
