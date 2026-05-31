using SkyRoute.Application.DTOs;
using SkyRoute.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyRoute.Application.Interfaces
{
    public interface IFlightProvider
    {
        string Provider { get; }
        Task<FlightProviderSearchResponse> SearchFlightsAsync(FlightSearchRequest request,CancellationToken cancellationToken = default);
        Task BookFlightAsync(string flightNumber,CancellationToken cancellationToken = default);
    }
}
