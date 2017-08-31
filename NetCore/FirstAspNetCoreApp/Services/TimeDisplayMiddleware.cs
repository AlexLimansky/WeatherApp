using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FirstAspNetCoreApp.Services
{
    public class TimeDisplayMiddleware
    {
        private readonly ILogger<TimeDisplayMiddleware> logger;
        private readonly RequestDelegate next;
        private bool current;

        public TimeDisplayMiddleware(RequestDelegate next, bool current, ILogger<TimeDisplayMiddleware> logger)
        {
            this.next = next;
            this.current = current;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context, IDateDisplayer displayer)
        {
            await next(context);
            string result = current ? displayer.DisplayStart() : displayer.DisplayCurrent();
            logger.LogInformation(current ? "TimeDisplayMiddleware invoked for MyDateDisplayer.DisplayStart() method." : "TimeDisplayMiddleware invoked for MyDateDisplayer.DisplayCurrent() method.");           
            await context.Response.WriteAsync(result);
            logger.LogDebug("TimeDisplayMiddleware returned " + result);
        }
    }
}
