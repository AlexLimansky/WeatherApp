using Microsoft.AspNetCore.Builder;

namespace FirstAspNetCoreApp.Services
{
    public static class TimeDisplayMiddlewareExtension
    {
        delegate string Del(int number);       
        public static IApplicationBuilder UseTimeDisplayer(this IApplicationBuilder builder)
        {
            builder.Map("/started", (x) => x.UseMiddleware<TimeDisplayMiddleware>(true));
            builder.UseMiddleware<TimeDisplayMiddleware>(false);
            return builder;
        }
    }
}
