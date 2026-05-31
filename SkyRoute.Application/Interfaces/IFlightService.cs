using SkyRoute.Application.DTOs;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Interfaces
{
    public interface IFlightService
    {
        // ── Search ──────────────────────────────────────────────────────────────
        Task<List<Flight>> SearchFlightsAsync(FlightSearchRequest request, CancellationToken cancellationToken = default);

        // ── Reference data ───────────────────────────────────────────────────────
        Task<FlightReferenceDataResponse> GetReferenceDataAsync(CancellationToken cancellationToken = default);

        // ── Persisted flights ────────────────────────────────────────────────────
        Task<List<Flight>> GetAllPersistedFlightsAsync(CancellationToken cancellationToken = default);
        Task<Flight?> GetFlightByNumberAsync(string flightNumber, CancellationToken cancellationToken = default);

        // ── Bookings ────────────────────────────────────────────────────────────
        Task<Booking> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken = default);
        Task<List<Booking>> GetAllBookingsAsync(CancellationToken cancellationToken = default);
        Task<Booking?> GetBookingByReferenceAsync(string bookingReference, CancellationToken cancellationToken = default);
    }
}
