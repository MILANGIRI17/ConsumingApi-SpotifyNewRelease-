using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace API.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
            string recordId,
            T data,
            TimeSpan? absoluteExpireTime=null,
            TimeSpan? unusedExpireTime=null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(1);
            options.SlidingExpiration = unusedExpireTime;

            var jsonData=JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<List<T>> GetRecordAsync<T>(this IDistributedCache cache,string recordId)
        {
            var jsonData= await cache.GetStringAsync(recordId);
            if(jsonData == null)
            {
                return default(List<T>);
            }
            return JsonSerializer.Deserialize<List<T>>(jsonData);
        }
    }
}
