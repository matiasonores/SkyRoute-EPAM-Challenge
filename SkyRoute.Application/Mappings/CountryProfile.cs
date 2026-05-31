using AutoMapper;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Mappings
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<string, Country>()

                .ForMember(
                    dest => dest.Code,
                    opt => opt.MapFrom(src => src))

                .ForMember(
                    dest => dest.Name,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.Id,
                    opt => opt.Ignore());
        }
    }
}