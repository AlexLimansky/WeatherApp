using Microsoft.Extensions.Options;
using NLog;
using NLog.Config;
using NLog.Targets;
using WeatherApp.Web.Options;

namespace WeatherApp.Web.Services
{
    public static class NLogConfigurator
    {
        public static LoggingConfiguration Configure(IOptions<LogOptions> options)
        {
            LogOptions option = options.Value;
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget
            {
                FileName = "${basedir}/log.txt",
                Layout = "${message}"
            };
            config.AddTarget("file", fileTarget);
            var ruleDefault = new LoggingRule("Weather*", LogLevel.FromString(option.Default), fileTarget);
            var ruleSystem = new LoggingRule("System", LogLevel.FromString(option.System), fileTarget);
            var ruleMicrosoft = new LoggingRule("Microsoft", LogLevel.FromString(option.Microsoft), fileTarget);
            config.LoggingRules.Add(ruleDefault);
            config.LoggingRules.Add(ruleSystem);
            config.LoggingRules.Add(ruleMicrosoft);
            return config;
        }
    }
}