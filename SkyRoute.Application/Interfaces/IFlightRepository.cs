using SkyRoute.Application.DTOs;
using SkyRoute.Application.Interfaces;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;

public interface IFlightRepository : IGenericRepository<Flight, Guid>
{
    Task<Flight?> GetByFlightNumberAsync(string flightNumber, CancellationToken cancellationToken = default);
    Task SaveFlightAsync(Flight flight, CancellationToken cancellationToken = default);
    Task<FlightReferenceDataResponse> GetReferenceDataAsync(CancellationToken cancellationToken = default);
    Task ClearFlightCacheAsync(CancellationToken cancellationToken = default);
    Task UpdateStatusAsync(Guid flightId, FlightStatus status, CancellationToken cancellationToken = default);
}
