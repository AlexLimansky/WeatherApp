using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace FirstAspNetCoreApp.Services
{
    public class MyConfigurator
    {
        public static LoggingConfiguration NLogConfigure(IConfigurationRoot configuration, IHostingEnvironment env)
        {
            LogLevel minLevel = configuration[env.EnvironmentName] != null ? LogLevel.FromString(configuration[env.EnvironmentName]) : LogLevel.FromString(configuration["Other"]);
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget
            {
                FileName = "${basedir}/log.txt",
                Layout = "${message}"
            };
            config.AddTarget("file", fileTarget);
            var rule = new LoggingRule("*", minLevel, fileTarget);
            config.LoggingRules.Add(rule);
            return config;
        }
        public static IConfigurationRoot OptionsConfigure(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}
