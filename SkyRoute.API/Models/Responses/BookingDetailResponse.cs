namespace SkyRoute.API.Models.Responses
{
    /// <summary>Full booking detail including flight and passenger information.</summary>
    public class BookingDetailResponse
    {
        /// <summary>Booking identifier.</summary>
        public Guid Id { get; set; }

        /// <summary>Short, human-readable booking reference.</summary>
        public string BookingReference { get; set; } = string.Empty;

        /// <summary>Complete flight details.</summary>
        public FlightResponse Flight { get; set; } = new();

        /// <summary>Price per passenger.</summary>
        public decimal Price { get; set; }

        /// <summary>Total amount charged for all passengers.</summary>
        public decimal TotalPrice { get; set; }

        /// <summary>Number of passengers on this booking.</summary>
        public int PassengerCount { get; set; }

        /// <summary>Booking creation timestamp (UTC).</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Booking status ("Pending", "Confirmed", "Cancelled").</summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>List of passengers on this booking.</summary>
        public List<PassengerResponse> Passengers { get; set; } = [];
    }
}
