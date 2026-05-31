using System.ComponentModel.DataAnnotations;

namespace SkyRoute.API.Models.Requests
{
    /// <summary>Flight search request sent from the Angular search form.</summary>
    public class SearchFlightsRequest : IValidatableObject
    {
        /// <summary>Number of adult passengers (≥ 1).</summary>
        [Range(1, 9)]
        public int Passengers { get; set; } = 1;

        /// <summary>
        /// Cabin class. Accepted values: "Economy", "Business", "First".
        /// </summary>
        [Required]
        public string CabinClass { get; set; } = "Economy";

        /// <summary>
        /// Flight type. Accepted values: "OneWay", "RoundTrip", "MultiCity".
        /// </summary>
        [Required]
        public string FlightType { get; set; } = "OneWay";

        /// <summary>When true, results include flights on adjacent dates (±1 day).</summary>
        public bool FlexDates { get; set; }

        /// <summary>One or more flight legs. Must contain at least one entry.</summary>
        [Required]
        [MinLength(1)]
        public List<FlightLegApiRequest> Legs { get; set; } = [];

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Enum.TryParse<SkyRoute.Domain.Enums.CabinClass>(CabinClass, ignoreCase: true, out _))
                yield return new ValidationResult(
                    $"'{CabinClass}' is not a valid cabin class. Accepted values: Economy, Business, First.",
                    [nameof(CabinClass)]);

            if (!Enum.TryParse<SkyRoute.Domain.Enums.FlightType>(FlightType, ignoreCase: true, out _))
                yield return new ValidationResult(
                    $"'{FlightType}' is not a valid flight type. Accepted values: OneWay, RoundTrip, MultiCity.",
                    [nameof(FlightType)]);

            foreach (var (leg, i) in Legs.Select((l, i) => (l, i)))
            {
                if (string.Equals(leg.Origin, leg.Destination, StringComparison.OrdinalIgnoreCase))
                    yield return new ValidationResult(
                        $"Leg {i + 1}: origin and destination airports must be different.",
                        [nameof(Legs)]);

                if (leg.DepartureDate < DateOnly.FromDateTime(DateTime.UtcNow))
                    yield return new ValidationResult(
                        $"Leg {i + 1}: departure date must be today or in the future.",
                        [nameof(Legs)]);
            }
        }
    }
}
