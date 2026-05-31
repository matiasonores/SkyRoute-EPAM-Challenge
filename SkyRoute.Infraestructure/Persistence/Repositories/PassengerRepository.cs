using Microsoft.EntityFrameworkCore;
using SkyRoute.Domain.Entities;
using SkyRoute.Infraestructure.Persistence;

namespace SkyRoute.Infraestructure.Persistence.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly SkyRouteDbContext _context;

        public PassengerRepository(SkyRouteDbContext context)
        {
            _context = context;
        }

        public async Task<Passenger?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Passengers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Passenger>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Passengers
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Passenger?> GetByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default)
        {
            return await _context.Passengers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.NationalId == nationalId, cancellationToken);
        }

        public async Task<Passenger?> GetByPassportNumberAsync(string passportNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Passengers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PassportNumber == passportNumber, cancellationToken);
        }

        public async Task<Passenger> CreateAsync(Passenger entity, CancellationToken cancellationToken = default)
        {
            _context.Passengers.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Passenger> UpdateAsync(Passenger entity, CancellationToken cancellationToken = default)
        {
            _context.Passengers.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var passenger = await _context.Passengers.FindAsync([id], cancellationToken);
            if (passenger is not null)
            {
                _context.Passengers.Remove(passenger);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
