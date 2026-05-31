using Microsoft.EntityFrameworkCore;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;
using SkyRoute.Infraestructure.Persistence;

namespace SkyRoute.Infraestructure.Persistence.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly SkyRouteDbContext _context;

        public BookingRepository(SkyRouteDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .Include(b => b.Flight)
                    .ThenInclude(f => f.Origin).ThenInclude(a => a.Country)
                .Include(b => b.Flight)
                    .ThenInclude(f => f.Destination).ThenInclude(a => a.Country)
                .Include(b => b.Passengers)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .Include(b => b.Flight)
                .Include(b => b.Passengers)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Booking?> GetByReferenceAsync(string bookingReference, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .Include(b => b.Flight)
                    .ThenInclude(f => f.Origin).ThenInclude(a => a.Country)
                .Include(b => b.Flight)
                    .ThenInclude(f => f.Destination).ThenInclude(a => a.Country)
                .Include(b => b.Passengers)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookingReference == bookingReference, cancellationToken);
        }

        public async Task<Booking> CreateAsync(Booking entity, CancellationToken cancellationToken = default)
        {
            // Attach existing passengers so EF does not attempt to insert them again
            foreach (var passenger in entity.Passengers)
            {
                if (_context.Entry(passenger).State == EntityState.Detached)
                    _context.Passengers.Attach(passenger);
            }

            _context.Bookings.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Booking> UpdateAsync(Booking entity, CancellationToken cancellationToken = default)
        {
            _context.Bookings.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateStatusAsync(Guid bookingId, BookingStatus status, CancellationToken cancellationToken = default)
        {
            await _context.Bookings
                .Where(b => b.Id == bookingId)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(b => b.Status, status),
                    cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var booking = await _context.Bookings.FindAsync([id], cancellationToken);
            if (booking is not null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
