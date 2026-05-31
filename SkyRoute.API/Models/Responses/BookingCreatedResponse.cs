namespace SkyRoute.API.Models.Responses
{
    /// <summary>Confirmation returned immediately after a successful booking creation (HTTP 201).</summary>
    public class BookingCreatedResponse
    {
        /// <summary>Short, human-readable booking reference (e.g. "A3F9C812").</summary>
        public string BookingReference { get; set; } = string.Empty;

        /// <summary>Flight number that was booked.</summary>
        public string FlightNumber { get; set; } = string.Empty;

        /// <summary>Total amount charged for all passengers.</summary>
        public decimal TotalPrice { get; set; }
    }
}
