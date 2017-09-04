using Microsoft.AspNetCore.Builder;

namespace FirstAspNetCoreApp.Services
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
        public static IApplicationBuilder UseTimeDisplayer(this IApplicationBuilder builder)
        {
            builder.Map("/started", (x) => x.UseMiddleware<TimeDisplayMiddleware>(true));
            builder.UseMiddleware<TimeDisplayMiddleware>(false);
            return builder;
        }
        public static IApplicationBuilder UseCache(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware(typeof(CacheMiddleware));
        }
        public static IApplicationBuilder UseCacheCleaner(this IApplicationBuilder builder)
        {
            return builder.UseWhen(c => c.Request.Path == "/clear", (x) => x.UseMiddleware<CacheClearMiddleware>());
        }
    }
}
