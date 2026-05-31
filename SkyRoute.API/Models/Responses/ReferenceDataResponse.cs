namespace SkyRoute.API.Models.Responses
{
    /// <summary>
    /// Reference data used to populate search dropdowns in the Angular frontend.
    /// Fetch this endpoint once on application startup and cache client-side.
    /// </summary>
    public class ReferenceDataResponse
    {
        /// <summary>All available countries.</summary>
        public List<CountryResponse> Countries { get; set; } = [];

        /// <summary>All available airports with their country associations.</summary>
        public List<AirportResponse> Airports { get; set; } = [];
    }
}
