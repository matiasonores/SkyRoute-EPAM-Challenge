using Microsoft.Extensions.Caching.Memory;
using SkyRoute.Application.Interfaces;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Infrastructure.Cache
{
    public class FlightCacheProvider : IFlightCacheProvider
    {
        private readonly IMemoryCache _cache;
        private readonly HashSet<string> _keys = new HashSet<string>();

        public FlightCacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<List<Flight>?> GetFlightsAsync(string cacheKey, CancellationToken cancellationToken = default)
        {
            _cache.TryGetValue(cacheKey, out List<Flight>? flights);

            return Task.FromResult(flights);
        }

        public Task SetFlightsAsync(string cacheKey, List<Flight> flights, TimeSpan expiration, CancellationToken cancellationToken = default)
        {
            _keys.Add(cacheKey);
            _cache.Set(cacheKey, flights, expiration);

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
        {
            _cache.Remove(cacheKey);
            _keys.Remove(cacheKey);

            return Task.CompletedTask;
        }

        public Task ClearAsync(CancellationToken cancellationToken = default)
        {
            foreach (var key in _keys)
            {
                _cache.Remove(key);
            }

            _keys.Clear();

            return Task.CompletedTask;
        }
    }
}