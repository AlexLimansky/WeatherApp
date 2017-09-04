using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace FirstAspNetCoreApp.Services
{
    public class CustomCache : IClearableCache
    {
        private List<object> keys = new List<object>();
        private IMemoryCache memoryCache;
        public CustomCache(IMemoryCache cache)
        {
            memoryCache = cache;
        }

        public void Clear()
        {
            foreach(var key in keys)
            {
                memoryCache.Remove(key);
            }
            keys = new List<object>();
        }

        public ICacheEntry CreateEntry(object key)
        {
            return memoryCache.CreateEntry(key);
        }

        public void Dispose()
        {
            memoryCache.Dispose();
        }

        public void Remove(object key)
        {
            keys.Remove(key);
            memoryCache.Remove(key);
        }

        public void Set(object key, object value, MemoryCacheEntryOptions options)
        {
            if(!keys.Contains(key))
            {
                keys.Add(key);
                memoryCache.Set(key, value, options);
            }           
        }

        public bool TryGetValue(object key, out object value)
        {
            value = null;
            object item = null;
            if(memoryCache.TryGetValue(key, out item))
            {
                value = item;
                return true;
            }
            return false;
        }
    }
}
