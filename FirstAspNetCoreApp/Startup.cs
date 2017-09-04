using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FirstAspNetCoreApp.Services;
using Microsoft.Extensions.Configuration;
using System;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.Extensions.Caching.Memory;

namespace FirstAspNetCoreApp
{
    public class Startup
    {
        public static IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private static void CreateError(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await next();
                throw new FieldAccessException("Test error was thrown");              
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMemoryCache();
            services.Configure<ResponseCacheOptions>(Configuration.GetSection("Cache"));
            services.AddScoped<IDateDisplayer, MyDateDisplayer>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSingleton<IClearableCache, CustomCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifetime, IDateDisplayer displayer)
        {
            var s = app.ApplicationServices.GetService<IClearableCache>();
            lifetime.ApplicationStarted.Register(() => displayer.SetStart());
            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog(MyConfigurator.NLogConfigure(Configuration, env));           
            app.UseErrorHandler();
            app.UseCacheCleaner();
            app.UseCache();
            app.Map("/throw", CreateError);                                            
            app.UseTimeDisplayer();           
        }
    }
}
