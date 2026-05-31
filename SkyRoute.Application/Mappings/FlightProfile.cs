using AutoMapper;
using SkyRoute.Application.DTOs;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.Mappings
{
    public class FlightProfile : Profile
    {
        public FlightProfile()
        {
            CreateMap<ProviderFlightResponse, Flight>()

                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(
                    dest => dest.Airline,
                    opt => opt.MapFrom(src => src.Airline))
                 .ForMember(
                    dest => dest.Provider,
                    opt => opt.MapFrom(src => src.Provider))
                .ForMember(
                    dest => dest.FlightNumber,
                    opt => opt.MapFrom(src => src.FlightCode))
                .ForMember(
                    dest => dest.Departure,
                    opt => opt.MapFrom(src => src.DepartureTime))
                .ForMember(
                    dest => dest.Arrival,
                    opt => opt.MapFrom(src => src.ArrivalTime))
                .ForMember(
                    dest => dest.Duration,
                    opt => opt.MapFrom(src =>
                        TimeSpan.FromMinutes(src.DurationMinutes)))
                .ForMember(
                    dest => dest.CabinClass,
                    opt => opt.MapFrom(src =>
                        Enum.Parse<CabinClass>(src.Cabin, true)))
                .ForMember(
                    dest => dest.Passengers,
                    opt => opt.MapFrom(src => src.Passengers))
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => src.PricePerPassenger))
                .ForMember(
                    dest => dest.TotalPrice,
                    opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(
                    dest => dest.IsInternational,
                    opt => opt.MapFrom(src =>
                        src.OriginCountry != src.DestinationCountry))
                .ForMember(
                    dest => dest.Origin,
                    opt => opt.MapFrom(src =>
                        new Airport
                        {
                            Code = src.OriginCode,
                            Name = src.OriginName,
                            Country = new Country
                            {
                                Code = src.OriginCountry
                            }
                        }))

                .ForMember(
                    dest => dest.Destination,
                    opt => opt.MapFrom(src =>
                        new Airport
                        {
                            Code = src.DestinationCode,
                            Name = src.DestinationName,
                            Country = new Country
                            {
                                Code = src.DestinationCountry
                            }
                        }))

                // FK IDs are resolved during persistence (SaveFlightAsync), not at mapping time
                .ForMember(dest => dest.OriginAirportId, opt => opt.Ignore())
                .ForMember(dest => dest.DestinationAirportId, opt => opt.Ignore());
        }
    }
}