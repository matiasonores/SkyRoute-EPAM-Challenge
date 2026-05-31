using System.ComponentModel.DataAnnotations;

namespace SkyRoute.API.Models.Requests
{
    /// <summary>A single flight leg (origin → destination on a given date).</summary>
    public class FlightLegApiRequest
    {
        /// <summary>IATA code of the departure airport (e.g. "SFN").</summary>
        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string Origin { get; set; } = string.Empty;

        /// <summary>IATA code of the arrival airport (e.g. "AEP").</summary>
        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string Destination { get; set; } = string.Empty;

        /// <summary>Requested departure date.</summary>
        [Required]
        public DateOnly DepartureDate { get; set; }
    }
}
