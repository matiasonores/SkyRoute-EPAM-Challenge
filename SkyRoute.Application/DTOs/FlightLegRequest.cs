using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.DTOs
{
    public class FlightLegRequest
    {
        public Airport Origin { get; set; }
        public Airport Destination { get; set; }
        public DateOnly DepartureDate { get; set; }
    }
}
