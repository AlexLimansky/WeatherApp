using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WeatherApp.Web
{
    public class Program
    {
#pragma warning disable Nix10 // The parameter is never used
        public static void Main(string[] args)
#pragma warning restore Nix10 // The parameter is never used
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}