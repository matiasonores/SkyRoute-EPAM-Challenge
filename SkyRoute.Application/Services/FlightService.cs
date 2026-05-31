using AutoMapper;
using SkyRoute.Application.DTOs;
using SkyRoute.Application.Interfaces;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IEnumerable<IFlightProvider> _flightProviders;
        private readonly IFlightRepository _flightRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IFlightCacheProvider _cacheProvider;
        private readonly IMapper _mapper;

        public FlightService(
            IEnumerable<IFlightProvider> flightProviders,
            IFlightRepository flightRepository,
            IBookingRepository bookingRepository,
            IPassengerRepository passengerRepository,
            IFlightCacheProvider cacheProvider,
            IMapper mapper)
        {
            _flightProviders = flightProviders;
            _flightRepository = flightRepository;
            _bookingRepository = bookingRepository;
            _passengerRepository = passengerRepository;
            _cacheProvider = cacheProvider;
            _mapper = mapper;
        }

        // ── Search ──────────────────────────────────────────────────────────────

        public async Task<List<Flight>> SearchFlightsAsync(FlightSearchRequest request, CancellationToken cancellationToken = default)
        {
            var cacheKey = BuildCacheKey(request);

            var cachedFlights = await _cacheProvider.GetFlightsAsync(cacheKey, cancellationToken);
            if (cachedFlights is not null)
                return cachedFlights;

            var providerTasks = _flightProviders.Select(x => x.SearchFlightsAsync(request, cancellationToken));
            var providerResponses = await Task.WhenAll(providerTasks);

            var flights = providerResponses
                .SelectMany(x => x.Flights)
                .Select(_mapper.Map<Flight>)
                .ToList();

            await _cacheProvider.SetFlightsAsync(cacheKey, flights, TimeSpan.FromMinutes(10), cancellationToken);

            return flights;
        }

        // ── Reference data ───────────────────────────────────────────────────────

        public async Task<FlightReferenceDataResponse> GetReferenceDataAsync(CancellationToken cancellationToken = default)
        {
            return await _flightRepository.GetReferenceDataAsync(cancellationToken);
        }

        // ── Persisted flights ────────────────────────────────────────────────────

        public async Task<List<Flight>> GetAllPersistedFlightsAsync(CancellationToken cancellationToken = default)
        {
            return await _flightRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Flight?> GetFlightByNumberAsync(string flightNumber, CancellationToken cancellationToken = default)
        {
            return await _flightRepository.GetByFlightNumberAsync(flightNumber, cancellationToken);
        }

        // ── Bookings ────────────────────────────────────────────────────────────

        public async Task<List<Booking>> GetAllBookingsAsync(CancellationToken cancellationToken = default)
        {
            return await _bookingRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Booking?> GetBookingByReferenceAsync(string bookingReference, CancellationToken cancellationToken = default)
        {
            return await _bookingRepository.GetByReferenceAsync(bookingReference, cancellationToken);
        }

        public async Task<Booking> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Flight);

            if (request.Passengers is null || request.Passengers.Count == 0)
                throw new InvalidOperationException("At least one passenger is required.");

            // Guard: flight must be Available to be booked
            var existingFlight = await _flightRepository.GetByFlightNumberAsync(
                request.Flight.FlightNumber, cancellationToken);

            // Deduplicate and persist passengers
            var persistedPassengers = new List<Passenger>();
            foreach (var passenger in request.Passengers)
            {
                Passenger? existing = null;

                if (!string.IsNullOrWhiteSpace(passenger.PassportNumber))
                    existing = await _passengerRepository.GetByPassportNumberAsync(passenger.PassportNumber, cancellationToken);

                if (existing is null && !string.IsNullOrWhiteSpace(passenger.NationalId))
                    existing = await _passengerRepository.GetByNationalIdAsync(passenger.NationalId, cancellationToken);

                if (existing is null)
                    existing = await _passengerRepository.CreateAsync(passenger, cancellationToken);

                persistedPassengers.Add(existing);
            }

            // Create booking in Pending state (transient — persisted as Confirmed below)
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                BookingReference = GenerateBookingReference(),
                FlightNumber = request.Flight.FlightNumber,
                FlightId = request.Flight.Id,
                Price = request.Flight.Price,
                TotalPrice = request.Price,
                PassengerCount = persistedPassengers.Count,
                CreatedAt = DateTime.UtcNow,
                Status = BookingStatus.Pending,
                Passengers = persistedPassengers
            };

            // Resolve and notify provider
            var provider = GetProvider(request.Flight.Provider);
            await provider.BookFlightAsync(request.Flight.FlightNumber, cancellationToken);

            // Persist flight as Reserved
            request.Flight.Status = FlightStatus.Reserved;
            await _flightRepository.SaveFlightAsync(request.Flight, cancellationToken);

            booking.Status = BookingStatus.Confirmed;
            booking.FlightId = request.Flight.Id;

            await _bookingRepository.CreateAsync(booking, cancellationToken);
            await _cacheProvider.ClearAsync(cancellationToken);

            return booking;
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        private static string BuildCacheKey(FlightSearchRequest request)
        {
            return string.Join(":", request.Legs.Select(x => $"{x.Origin.Code}-{x.Destination.Code}-{x.DepartureDate}"))
                + $":{request.CabinClass}:{request.Passengers}";
        }

        private static string GenerateBookingReference()
        {
            return Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
        }

        private IFlightProvider GetProvider(string providerName)
        {
            return _flightProviders.FirstOrDefault(x =>
                x.Provider.Equals(providerName, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidOperationException($"Provider '{providerName}' not found.");
        }
    }
}
