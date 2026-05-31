using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.DTOs
{
    public class FlightReferenceDataResponse
    {
        public List<Airport> Airports { get; set; } = [];
    }
}
