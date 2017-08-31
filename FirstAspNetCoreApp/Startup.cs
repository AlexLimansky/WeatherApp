using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FirstAspNetCoreApp.Services;
using Microsoft.Extensions.Configuration;
using System;
using NLog.Extensions.Logging;
using NLog.Web;

namespace FirstAspNetCoreApp
{
    public class Startup
    {
        public static IConfigurationRoot Configuration { get; set; }

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
            services.AddScoped<IDateDisplayer, MyDateDisplayer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifetime, IDateDisplayer displayer)
        {
            Configuration = MyConfigurator.OptionsConfigure(env);
            lifetime.ApplicationStarted.Register(() => displayer.SetStart());
            app.UseDeveloperExceptionPage();
            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog(MyConfigurator.NLogConfigure(Configuration, env));
            app.UseErrorHandler();
            app.Map("/throw", CreateError);                        
            app.UseTimeDisplayer();
            var logger = loggerFactory.CreateLogger("NetCoreMVCLogger");
            logger.LogError(env.EnvironmentName);
        }
    }
}
