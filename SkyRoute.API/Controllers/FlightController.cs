using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.API.Models.Requests;
using SkyRoute.API.Models.Responses;
using SkyRoute.Application.Interfaces;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;
using SkyRoute.Application.DTOs;

namespace SkyRoute.API.Controllers
{
    /// <summary>
    /// Flight search, reservation, and booking lifecycle management.
    /// </summary>
    [ApiController]
    [Route("api/flights")]
    [Produces("application/json")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;

        public FlightController(IFlightService flightService, IMapper mapper)
        {
            _flightService = flightService;
            _mapper = mapper;
        }

        // ── Search ────────────────────────────────────────────────────────────────

        /// <summary>Search available flights across all configured providers.</summary>
        /// <remarks>
        /// Aggregates results from GlobalAir and BudgetWings. Results may be served from
        /// a 10-minute in-memory cache for the same search parameters.
        ///
        /// The returned FlightResponse objects must be passed back unmodified when
        /// creating a booking via POST /api/flights/bookings.
        ///
        ///     POST /api/flights/search
        ///     {
        ///       "passengers": 1,
        ///       "cabinClass": "Economy", "flightType": "OneWay", "flexDates": false,
        ///       "legs": [{ "origin": "SFN", "destination": "AEP", "departureDate": "2026-07-01" }]
        ///     }
        /// </remarks>
        [HttpPost("search")]
        [ProducesResponseType(typeof(List<FlightResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchFlights(
            [FromBody] SearchFlightsRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var refData = await _flightService.GetReferenceDataAsync(cancellationToken);
            var airportsByCode = refData.Airports.ToDictionary(a => a.Code, StringComparer.OrdinalIgnoreCase);

            var appRequest = new FlightSearchRequest
            {
                Passengers = request.Passengers,
                CabinClass = Enum.Parse<CabinClass>(request.CabinClass, ignoreCase: true),
                FlightType = Enum.Parse<FlightType>(request.FlightType, ignoreCase: true),
                FlexDates = request.FlexDates,
                Legs = request.Legs.Select(leg =>
                {
                    var origin = airportsByCode.GetValueOrDefault(leg.Origin)
                        ?? new Airport { Code = leg.Origin, Name = leg.Origin, Country = new Country { Code = "XX", Name = leg.Origin } };
                    var destination = airportsByCode.GetValueOrDefault(leg.Destination)
                        ?? new Airport { Code = leg.Destination, Name = leg.Destination, Country = new Country { Code = "XX", Name = leg.Destination } };
                    return new FlightLegRequest { Origin = origin, Destination = destination, DepartureDate = leg.DepartureDate };
                }).ToList()
            };

            var flights = await _flightService.SearchFlightsAsync(appRequest, cancellationToken);
            return Ok(_mapper.Map<List<FlightResponse>>(flights));
        }

        // ── Reference data ────────────────────────────────────────────────────────

        /// <summary>Returns all countries and airports available for flight search.</summary>
        /// <remarks>
        /// Fetch once on Angular app startup and cache client-side for the session lifetime.
        ///
        ///     GET /api/flights/reference-data
        /// </remarks>
        [HttpGet("reference-data")]
        [ProducesResponseType(typeof(ReferenceDataResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReferenceData(CancellationToken cancellationToken)
        {
            var data = await _flightService.GetReferenceDataAsync(cancellationToken);
            var response = new ReferenceDataResponse
            {
                Airports = _mapper.Map<List<AirportResponse>>(data.Airports),
                Countries = _mapper.Map<List<CountryResponse>>(
                    data.Airports
                        .Where(a => a.Country is not null)
                        .Select(a => a.Country)
                        .DistinctBy(c => c.Id)
                        .ToList())
            };
            return Ok(response);
        }

        // ── Persisted flights ─────────────────────────────────────────────────────

        /// <summary>Returns all persisted flights regardless of status.</summary>
        /// <remarks>
        /// Only flights that have been committed through a booking appear here.
        /// Transient search results are not persisted until a booking is created.
        ///
        ///     GET /api/flights/persisted
        /// </remarks>
        [HttpGet("persisted")]
        [ProducesResponseType(typeof(List<FlightResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPersistedFlights(CancellationToken cancellationToken)
        {
            var flights = await _flightService.GetAllPersistedFlightsAsync(cancellationToken);
            return Ok(_mapper.Map<List<FlightResponse>>(flights));
        }

        /// <summary>Returns a single flight by its flight number.</summary>
        /// <remarks>
        /// Only flights persisted via a booking are retrievable from the database.
        ///
        ///     GET /api/flights/GA100042
        /// </remarks>
        /// <param name="flightNumber">Flight code (e.g. "GA100042").</param>
        [HttpGet("{flightNumber}")]
        [ProducesResponseType(typeof(FlightResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFlightByNumber(
            string flightNumber,
            CancellationToken cancellationToken)
        {
            var flight = await _flightService.GetFlightByNumberAsync(flightNumber, cancellationToken);
            if (flight is null)
                return NotFound(new ProblemDetails
                {
                    Status   = StatusCodes.Status404NotFound,
                    Title    = "Flight Not Found",
                    Detail   = $"No flight with number '{flightNumber}' was found.",
                    Instance = HttpContext.Request.Path
                });
            return Ok(_mapper.Map<FlightResponse>(flight));
        }

        // ── Bookings ──────────────────────────────────────────────────────────────

        /// <summary>Returns all bookings regardless of status.</summary>
        /// <remarks>
        /// Includes Pending, Confirmed, and Cancelled bookings.
        ///
        ///     GET /api/flights/bookings
        /// </remarks>
        [HttpGet("bookings")]
        [ProducesResponseType(typeof(List<BookingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookings(CancellationToken cancellationToken)
        {
            var bookings = await _flightService.GetAllBookingsAsync(cancellationToken);
            return Ok(_mapper.Map<List<BookingResponse>>(bookings));
        }

        /// <summary>Creates a new booking for a selected flight.</summary>
        /// <remarks>
        /// Pass the FlightResponse from POST /api/flights/search back in the flight field.
        /// Returns 201 Created with a Location header pointing to the booking detail.
        ///
        ///     POST /api/flights/bookings
        ///     {
        ///       "flight": { ...FlightResponse from search... },
        ///       "price": 230.00,
        ///       "passengers": [{ "fullName": "Jane Doe", "emailAddress": "jane@example.com", "passportNumber": "AR999" }]
        ///     }
        /// </remarks>
        [HttpPost("bookings")]
        [ProducesResponseType(typeof(BookingCreatedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBooking([FromBody] Models.Requests.CreateBookingRequest request, 
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (request.Price <= 0)
                return ValidationProblem(detail: "Price must be greater than zero.", title: "Invalid Price");

            var flight = _mapper.Map<Flight>(request.Flight);
            var passengers = _mapper.Map<List<Passenger>>(request.Passengers);

            var appRequest = new Application.DTOs.CreateBookingRequest
            {
                Flight = flight,
                Price = request.Price,
                Passengers = passengers
            };

            var booking = await _flightService.CreateBookingAsync(appRequest, cancellationToken);
            var response = _mapper.Map<BookingCreatedResponse>(booking);

            return CreatedAtAction(
                actionName: nameof(GetBookingByReference),
                routeValues: new { bookingReference = booking.BookingReference },
                value: response);
        }

        /// <summary>Returns full booking detail including flight and passengers.</summary>
        /// <remarks>
        ///     GET /api/flights/bookings/A3F9C812
        /// </remarks>
        /// <param name="bookingReference">8-character booking reference (e.g. "A3F9C812").</param>
        [HttpGet("bookings/{bookingReference}")]
        [ProducesResponseType(typeof(BookingDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookingByReference(string bookingReference, 
            CancellationToken cancellationToken)
        {
            var booking = await _flightService.GetBookingByReferenceAsync(bookingReference, cancellationToken);
            if (booking is null)
                return NotFound(new ProblemDetails
                {
                    Status   = StatusCodes.Status404NotFound,
                    Title    = "Booking Not Found",
                    Detail   = $"No booking with reference '{bookingReference}' was found.",
                    Instance = HttpContext.Request.Path
                });
            return Ok(_mapper.Map<BookingDetailResponse>(booking));
        }

    }
}
