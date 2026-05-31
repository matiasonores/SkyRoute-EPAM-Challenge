namespace SkyRoute.API.Models.Responses
{
    /// <summary>Booking summary used in list responses.</summary>
    public class BookingResponse
    {
        /// <summary>Booking identifier.</summary>
        public Guid Id { get; set; }

        /// <summary>Short, human-readable booking reference.</summary>
        public string BookingReference { get; set; } = string.Empty;

        /// <summary>Booked flight number.</summary>
        public string FlightNumber { get; set; } = string.Empty;

        /// <summary>Total amount charged.</summary>
        public decimal TotalPrice { get; set; }

        /// <summary>Booking creation timestamp (UTC).</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Booking status ("Pending", "Confirmed", "Cancelled").</summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>Number of passengers on this booking.</summary>
        public int PassengerCount { get; set; }
    }
}
