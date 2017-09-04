using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace FirstAspNetCoreApp.Services
{
    public class CacheMiddleware
    {
        private IClearableCache cache;
        private readonly ResponseCacheOptions options;
        private readonly RequestDelegate next;
        private readonly ILogger<CacheMiddleware> logger;

        public CacheMiddleware(RequestDelegate next, ILogger<CacheMiddleware> logger, IOptions<ResponseCacheOptions> options, IClearableCache memoryCache)
        {
            this.options = options.Value;
            this.next = next;
            this.logger = logger;
            cache = memoryCache;
        }

        public async Task Invoke(HttpContext context)
        {
            string cacheEntry;
            if (!cache.TryGetValue(context.Request.Path, out cacheEntry))
            {                                
                await next(context);
                cacheEntry = context.Items["result"].ToString();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(options.Lifetime));
                cache.Set(context.Request.Path, cacheEntry, cacheEntryOptions);
                logger.LogInformation("CacheMiddleware saved response");
            }
            else
            {
                logger.LogInformation("CacheMiddleware replaced response");
                await context.Response.WriteAsync(cacheEntry);          
            }           
        }
    }
}
