using AutoMapper;
using SkyRoute.Application.DTOs;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Mappings
{
    public class AirportProfile : Profile
    {
        public AirportProfile()
        {
            CreateMap<ProviderFlightResponse, Airport>()

                .ForMember(
                    dest => dest.Code,
                    opt => opt.MapFrom(src => src.DestinationCode))

                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.DestinationName))

                .ForMember(d => d.Country, opt => opt.MapFrom(s => new Country { Name = s.DestinationCountry }));
        }
    }
}