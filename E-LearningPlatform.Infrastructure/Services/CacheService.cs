using E_LearningPlatform.Application.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        public CacheService (IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<T?> GetAsync<T> (string key, CancellationToken cancellationToken = default)
        {
            var json =
            await _cache.GetStringAsync(key, cancellationToken);
            if (json == null)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(json);
        }



        public async Task SetAsync<T> (string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions
            { AbsoluteExpirationRelativeToNow = expiration };

            await _cache.SetStringAsync(
                key,
                JsonSerializer.Serialize(value),
                options,
                cancellationToken);
        }

        public async Task RemoveAsync (
            string key,
            CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(
                key,
                cancellationToken);
        }
    }


}

