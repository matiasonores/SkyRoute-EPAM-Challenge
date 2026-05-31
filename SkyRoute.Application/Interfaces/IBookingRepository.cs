using SkyRoute.Application.Interfaces;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;

public interface IBookingRepository : IGenericRepository<Booking, Guid>
{
    Task<Booking?> GetByReferenceAsync(string bookingReference, CancellationToken cancellationToken = default);
    Task UpdateStatusAsync(Guid bookingId, BookingStatus status, CancellationToken cancellationToken = default);
}
