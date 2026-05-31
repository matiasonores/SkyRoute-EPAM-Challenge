namespace SkyRoute.API.Models.Responses
{
    /// <summary>Airport reference data used in search dropdowns and flight details.</summary>
    public class AirportResponse
    {
        /// <summary>IATA airport code (e.g. "AEP", "SFN").</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Airport full name.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>City the airport serves. May be null for smaller airports.</summary>
        public string? City { get; set; }

        /// <summary>Country the airport is located in.</summary>
        public CountryResponse Country { get; set; } = new();
    }
}
