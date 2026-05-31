using Microsoft.EntityFrameworkCore;
using SkyRoute.Application.DTOs;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;
using SkyRoute.Infraestructure.Persistence;

namespace SkyRoute.Infraestructure.Persistence.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly SkyRouteDbContext _context;

        public FlightRepository(SkyRouteDbContext context)
        {
            _context = context;
        }

        public async Task<Flight?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Flights
                .Include(f => f.Origin).ThenInclude(a => a.Country)
                .Include(f => f.Destination).ThenInclude(a => a.Country)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        }

        public async Task<List<Flight>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Flights
                .Include(f => f.Origin).ThenInclude(a => a.Country)
                .Include(f => f.Destination).ThenInclude(a => a.Country)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Flight?> GetByFlightNumberAsync(string flightNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Flights
                .Include(f => f.Origin).ThenInclude(a => a.Country)
                .Include(f => f.Destination).ThenInclude(a => a.Country)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.FlightNumber == flightNumber, cancellationToken);
        }

        public async Task<FlightReferenceDataResponse> GetReferenceDataAsync(CancellationToken cancellationToken = default)
        {
            var airports = await _context.Airports
                .Include(a => a.Country)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new FlightReferenceDataResponse { Airports = airports };
        }

        // Upserts country → airport → flight so providers can persist transient entities
        // without creating duplicates across repeated searches.
        // If a flight with the same FlightNumber already exists, the caller's flight.Id is
        // overwritten with the persisted Id so that Booking.FlightId always references a
        // real Flights row (prevents FK violations on second+ bookings of the same flight).
        // The flight.Status on the caller object is also applied to the existing record when changed.
        public async Task SaveFlightAsync(Flight flight, CancellationToken cancellationToken = default)
        {
            flight.OriginAirportId = await UpsertAirportAsync(flight.Origin, cancellationToken);
            flight.DestinationAirportId = await UpsertAirportAsync(flight.Destination, cancellationToken);

            var existing = await _context.Flights
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.FlightNumber == flight.FlightNumber, cancellationToken);

            if (existing is not null)
            {
                // Sync the Id so the caller can safely use it as a booking FK
                flight.Id = existing.Id;

                // Propagate the status change when the caller has set a new status
                if (existing.Status != flight.Status)
                    await UpdateStatusAsync(existing.Id, flight.Status, cancellationToken);

                return;
            }

            // Detach navigation objects so EF does not attempt to insert them again
            flight.Origin = null!;
            flight.Destination = null!;

            _context.Flights.Add(flight);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Flight> CreateAsync(Flight entity, CancellationToken cancellationToken = default)
        {
            _context.Flights.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Flight> UpdateAsync(Flight entity, CancellationToken cancellationToken = default)
        {
            _context.Flights.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateStatusAsync(Guid flightId, FlightStatus status, CancellationToken cancellationToken = default)
        {
            await _context.Flights
                .Where(f => f.Id == flightId)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(f => f.Status, status),
                    cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var flight = await _context.Flights.FindAsync([id], cancellationToken);
            if (flight is not null)
            {
                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        // Cache clearing is handled by IFlightCacheProvider; nothing to do at the DB level.
        public Task ClearFlightCacheAsync(CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        private async Task<int> UpsertAirportAsync(Airport airport, CancellationToken cancellationToken)
        {
            var existing = await _context.Airports
                .FirstOrDefaultAsync(a => a.Code == airport.Code, cancellationToken);

            if (existing is not null)
                return existing.Id;

            int countryId = await UpsertCountryAsync(airport.Country, cancellationToken);

            var newAirport = new Airport
            {
                Code = airport.Code,
                Name = airport.Name,
                City = airport.City,
                CountryId = countryId
            };

            _context.Airports.Add(newAirport);
            await _context.SaveChangesAsync(cancellationToken);

            return newAirport.Id;
        }

        private async Task<int> UpsertCountryAsync(Country country, CancellationToken cancellationToken)
        {
            var existing = await _context.Countries
                .FirstOrDefaultAsync(c => c.Code == country.Code, cancellationToken);

            if (existing is not null)
                return existing.Id;

            // Name is often unavailable from provider responses; default to the code.
            var newCountry = new Country
            {
                Code = country.Code,
                Name = string.IsNullOrWhiteSpace(country.Name) ? country.Code : country.Name
            };

            _context.Countries.Add(newCountry);
            await _context.SaveChangesAsync(cancellationToken);

            return newCountry.Id;
        }
    }
}
