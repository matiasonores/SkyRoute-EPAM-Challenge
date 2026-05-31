namespace SkyRoute.API.Models.Responses
{
    /// <summary>Passenger detail including travel document numbers for booking context.</summary>
    public class PassengerResponse
    {
        /// <summary>Passenger identifier.</summary>
        public Guid Id { get; set; }

        /// <summary>Full name as it appears on the travel document.</summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>Contact email address.</summary>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>National identity document number. Shown for domestic flights.</summary>
        public string? NationalId { get; set; }

        /// <summary>Passport number. Shown for international flights.</summary>
        public string? PassportNumber { get; set; }
    }
}
