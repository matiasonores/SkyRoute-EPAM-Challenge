using System.ComponentModel.DataAnnotations;

namespace SkyRoute.API.Models.Requests
{
    /// <summary>Passenger details required at booking time.</summary>
    public class PassengerRequest
    {
        /// <summary>Full name exactly as it appears on the travel document.</summary>
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        /// <summary>Contact email address.</summary>
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// National identity document number.
        /// At least one of NationalId or PassportNumber must be provided.
        /// </summary>
        [StringLength(50)]
        public string? NationalId { get; set; }

        /// <summary>
        /// Passport number. Required for international flights.
        /// At least one of NationalId or PassportNumber must be provided.
        /// </summary>
        [StringLength(50)]
        public string? PassportNumber { get; set; }
    }
}
