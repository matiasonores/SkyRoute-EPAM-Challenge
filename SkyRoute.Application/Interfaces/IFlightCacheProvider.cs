using SkyRoute.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyRoute.Application.Interfaces
{
    public interface IFlightCacheProvider
    {
        Task<List<Flight>?> GetFlightsAsync(string cacheKey, CancellationToken cancellationToken = default);
        Task SetFlightsAsync(string cacheKey,List<Flight> flights,TimeSpan expiration,CancellationToken cancellationToken = default);
        Task RemoveAsync(string cacheKey,CancellationToken cancellationToken = default);
        Task ClearAsync(CancellationToken cancellationToken = default);
    }
}
