namespace SkyRoute.API.Models.Responses
{
    /// <summary>Country reference data.</summary>
    public class CountryResponse
    {
        /// <summary>ISO 3166-1 alpha-2 country code (e.g. "AR", "US").</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Full country name.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
