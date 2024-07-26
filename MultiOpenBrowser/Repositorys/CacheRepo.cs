using MultiOpenBrowser.Entitys;
using Newtonsoft.Json;

namespace MultiOpenBrowser.Repositorys
{
    internal class CacheRepo(IUnitOfWork? uow) : BaseRepo<Cache>(uow, null, null)
    {
        public static async Task SetAsync(string key, object? value, DateTimeOffset? expired, CancellationToken cancellationToken = default)
        {
            if (value == null)
            {
                return;
            }
            string? cacheValue;
            if (value.GetType() == typeof(string))
            {
                cacheValue = value.ToString();
            }
            else
            {
                cacheValue = JsonConvert.SerializeObject(value);
            }
            CacheRepo cacheRepo = new(null);
            Cache cache = new()
            {
                Key = key,
                Value = cacheValue,
                Expired = expired,
            };
            await cacheRepo.InsertOrUpdateAsync(cache, cancellationToken);
        }

        public static async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            CacheRepo cacheRepo = new(null);
            var cache = await cacheRepo.Select.Where(a => a.Key == key).FirstAsync(cancellationToken);
            if (cache == null || cache.Expired != null && cache.Expired < DateTimeOffset.Now)
            {
                return default;
            }
            if (string.IsNullOrEmpty(cache.Value))
            {
                return default;
            }
            if (typeof(T) == typeof(string))
            {
                return (T)(object)cache.Value;
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(cache.Value);
            }
        }

        public static string? Get(string key)
        {
            CacheRepo cacheRepo = new(null);
            var cache = cacheRepo.Select.Where(a => a.Key == key).First();
            if (cache == null || cache.Expired != null && cache.Expired < DateTimeOffset.Now)
            {
                return default;
            }
            return cache.Value;
        }

        public static T? Get<T>(string key)
        {
            CacheRepo cacheRepo = new(null);
            var cache = cacheRepo.Select.Where(a => a.Key == key).First();
            if (cache == null || cache.Expired != null && cache.Expired < DateTimeOffset.Now)
            {
                return default;
            }
            if (string.IsNullOrEmpty(cache.Value))
            {
                return default;
            }
            if (typeof(T) == typeof(string))
            {
                return (T)(object)cache.Value;
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(cache.Value);
            }
        }
    }
}
