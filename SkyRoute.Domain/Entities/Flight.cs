using SkyRoute.Domain.Entities.BaseEntities;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Domain.Entities
{
    public class Flight : BaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Airline { get; set; }
        public string Provider { get; set; }
        public string FlightNumber { get; set; }
        public int OriginAirportId { get; set; }
        public Airport Origin { get; set; }
        public int DestinationAirportId { get; set; }
        public Airport Destination { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public TimeSpan Duration { get; set; }
        public CabinClass CabinClass { get; set; }
        public int Passengers { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsInternational { get; set; }
        public FlightStatus Status { get; set; } = FlightStatus.Available;
    }
}
