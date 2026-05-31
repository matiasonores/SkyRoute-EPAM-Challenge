using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.DTOs
{
    public class FlightProviderSettings
    {
        public string ProviderName { get; set; } = string.Empty;
        public string FlightPrefix { get; set; } = string.Empty;
        public int MinFlights { get; set; }
        public int MaxFlights { get; set; }
        public int MinDurationMinutes { get; set; }
        public int MaxDurationMinutes { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public List<CabinClass> SupportedCabins { get; set; } = [];
    }
}