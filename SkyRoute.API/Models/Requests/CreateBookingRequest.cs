using SkyRoute.API.Models.Responses;
using System.ComponentModel.DataAnnotations;

namespace SkyRoute.API.Models.Requests
{
    /// <summary>
    /// Request body for creating a new booking.
    /// The <see cref="Flight"/> object should be the exact <see cref="FlightResponse"/>
    /// received from the search endpoint — the server uses it to reconstruct and persist
    /// the transient provider flight before committing the booking.
    /// </summary>
    public class CreateBookingRequest : IValidatableObject
    {
        /// <summary>
        /// The flight to book.
        /// Pass back the FlightResponse object returned from POST /search.
        /// </summary>
        [Required]
        public FlightResponse Flight { get; set; } = new();

        /// <summary>
        /// Total price the client confirmed. Must match the flight's TotalPrice within a tolerance.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        /// <summary>One or more passengers to include on the booking.</summary>
        [Required]
        [MinLength(1, ErrorMessage = "At least one passenger is required.")]
        public List<PassengerRequest> Passengers { get; set; } = [];

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Flight is not null && Passengers.Count != Flight.PassengerCount)
                yield return new ValidationResult(
                    $"The number of passengers ({Passengers.Count}) must match the flight's passenger count ({Flight.PassengerCount}).",
                    [nameof(Passengers)]);

            foreach (var (p, i) in Passengers.Select((p, i) => (p, i)))
            {
                if (string.IsNullOrWhiteSpace(p.NationalId) && string.IsNullOrWhiteSpace(p.PassportNumber))
                    yield return new ValidationResult(
                        $"Passenger {i + 1}: at least one of NationalId or PassportNumber must be provided.",
                        [nameof(Passengers)]);
            }
        }
    }
}
