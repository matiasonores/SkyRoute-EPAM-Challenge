namespace SkyRoute.API.Models.Responses
{
    /// <summary>
    /// A flight result from a provider search.
    /// This object is also sent back in the body of POST /bookings to create a reservation.
    /// </summary>
    public class FlightResponse
    {
        /// <summary>Unique flight identifier generated at search time.</summary>
        public Guid Id { get; set; }

        /// <summary>Provider name (e.g. "GlobalAir", "BudgetWings").</summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>Airline operating the flight.</summary>
        public string Airline { get; set; } = string.Empty;

        /// <summary>Flight code (e.g. "GA100042").</summary>
        public string FlightNumber { get; set; } = string.Empty;

        /// <summary>Origin airport.</summary>
        public AirportResponse Origin { get; set; } = new();

        /// <summary>Destination airport.</summary>
        public AirportResponse Destination { get; set; } = new();

        /// <summary>Scheduled departure (UTC).</summary>
        public DateTime Departure { get; set; }

        /// <summary>Scheduled arrival (UTC).</summary>
        public DateTime Arrival { get; set; }

        /// <summary>Flight duration in total minutes (e.g. 150 = 2 h 30 m).</summary>
        public int DurationMinutes { get; set; }

        /// <summary>Cabin class as a string ("Economy", "Business", "First").</summary>
        public string CabinClass { get; set; } = string.Empty;

        /// <summary>Number of passengers included in the price.</summary>
        public int PassengerCount { get; set; }

        /// <summary>Price per passenger.</summary>
        public decimal Price { get; set; }

        /// <summary>Total price for all passengers.</summary>
        public decimal TotalPrice { get; set; }

        /// <summary>True when origin and destination are in different countries.</summary>
        public bool IsInternational { get; set; }
    }
}
