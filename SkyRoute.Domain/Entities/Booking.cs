using SkyRoute.Domain.Entities.BaseEntities;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Domain.Entities
{
    public class Booking : BaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public string BookingReference { get; set; }
        public string FlightNumber { get; set; }
        public Guid FlightId { get; set; }
        public int PassengerCount { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public BookingStatus Status { get; set; }
        public Flight Flight { get; set; }
        public List<Passenger> Passengers { get; set; }
    }
}
