using Microsoft.Extensions.Caching.Memory;

namespace FirstAspNetCoreApp.Services
{
    public interface IClearableCache : IMemoryCache
    {
        void Set(object key, object value, MemoryCacheEntryOptions options);
        void Clear();
    }
}
