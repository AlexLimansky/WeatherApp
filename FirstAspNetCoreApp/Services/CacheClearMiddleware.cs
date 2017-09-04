using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FirstAspNetCoreApp.Services
{
    public class CacheClearMiddleware
    {
        private IClearableCache cache;
        private readonly RequestDelegate next;
        private readonly ILogger<CacheMiddleware> logger;

        public CacheClearMiddleware(RequestDelegate next, ILogger<CacheMiddleware> logger, IClearableCache memoryCache)
        {
            this.next = next;
            this.logger = logger;
            cache = memoryCache;
        }

        public async Task Invoke(HttpContext context)
        {
            await next(context);
            cache.Clear();
            logger.LogInformation("Cache was dropped");
        }
    }
}
