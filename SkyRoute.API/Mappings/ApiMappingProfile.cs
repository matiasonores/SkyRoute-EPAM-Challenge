using AutoMapper;
using SkyRoute.API.Models.Responses;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;

namespace SkyRoute.API.Mappings
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            // ── Reference data ────────────────────────────────────────────────────────
            CreateMap<Country, CountryResponse>();

            CreateMap<Airport, AirportResponse>()
                .ForMember(d => d.Country, opt => opt.MapFrom(src => src.Country));

            // ── Flight: domain entity → API response ──────────────────────────────────
            CreateMap<Flight, FlightResponse>()
                .ForMember(d => d.FlightNumber,     opt => opt.MapFrom(src => src.FlightNumber))
                .ForMember(d => d.PassengerCount,   opt => opt.MapFrom(src => src.Passengers))
                .ForMember(d => d.DurationMinutes,  opt => opt.MapFrom(src => (int)src.Duration.TotalMinutes))
                .ForMember(d => d.CabinClass,       opt => opt.MapFrom(src => src.CabinClass.ToString()))
                .ForMember(d => d.Origin,           opt => opt.MapFrom(src => src.Origin))
                .ForMember(d => d.Destination,      opt => opt.MapFrom(src => src.Destination));

            // ── Flight: API response → domain entity (used when booking a search result) ──
            // The client sends back the FlightResponse it received; we reconstruct the entity
            // so SaveFlightAsync can upsert it before creating the booking FK.
            CreateMap<FlightResponse, Flight>()
                .ForMember(d => d.Passengers,           opt => opt.MapFrom(src => src.PassengerCount))
                .ForMember(d => d.Duration,             opt => opt.MapFrom(src => TimeSpan.FromMinutes(src.DurationMinutes)))
                .ForMember(d => d.CabinClass,           opt => opt.MapFrom(src => Enum.Parse<CabinClass>(src.CabinClass, true)))
                .ForMember(d => d.OriginAirportId,      opt => opt.Ignore())
                .ForMember(d => d.DestinationAirportId, opt => opt.Ignore())
                .ForMember(d => d.Origin,               opt => opt.MapFrom(src => src.Origin))
                .ForMember(d => d.Destination,          opt => opt.MapFrom(src => src.Destination));

            // AirportResponse → Airport (used in FlightResponse → Flight above)
            CreateMap<AirportResponse, Airport>()
                .ForMember(d => d.CountryId, opt => opt.Ignore())
                .ForMember(d => d.Country,   opt => opt.MapFrom(src => src.Country));

            CreateMap<CountryResponse, Country>()
                .ForMember(d => d.Id, opt => opt.Ignore());

            // ── Booking: domain entity → API responses ────────────────────────────────
            CreateMap<Booking, BookingCreatedResponse>()
                .ForMember(d => d.FlightNumber, opt => opt.MapFrom(src => src.FlightNumber));

            CreateMap<Booking, BookingResponse>()
                .ForMember(d => d.Status,         opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(d => d.PassengerCount, opt => opt.MapFrom(src => src.Passengers != null
                    ? src.Passengers.Count
                    : src.PassengerCount));

            CreateMap<Booking, BookingDetailResponse>()
                .ForMember(d => d.Status,         opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(d => d.Flight,         opt => opt.MapFrom(src => src.Flight))
                .ForMember(d => d.Passengers,     opt => opt.MapFrom(src => src.Passengers))
                .ForMember(d => d.PassengerCount, opt => opt.MapFrom(src => src.Passengers != null
                    ? src.Passengers.Count
                    : src.PassengerCount));

            // ── Passenger: domain entity → API response ───────────────────────────────
            CreateMap<Passenger, PassengerResponse>();

            // ── Passenger: API request → domain entity ────────────────────────────────
            CreateMap<Models.Requests.PassengerRequest, Passenger>()
                .ForMember(d => d.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));
        }
    }
}
