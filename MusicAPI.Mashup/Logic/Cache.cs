namespace MusicAPI.Mashup.Logic
{
    using System;
    using System.Runtime.Caching;
    using System.Threading.Tasks;

    public static class Cache
    {
        public static async Task<T> GetObjectFromCache<T>(
            string cacheItemName,
            int cacheTimeInMinutes,
            Func<Task<T>> getObjectFunction)
        {
            ObjectCache cache = MemoryCache.Default;
            var cachedObject = (T)cache[cacheItemName];

            if (cachedObject != null) return cachedObject;

            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheTimeInMinutes) };

            cachedObject = await getObjectFunction();
            cache.Set(cacheItemName, cachedObject, policy);
            return cachedObject;
        }
    }
}