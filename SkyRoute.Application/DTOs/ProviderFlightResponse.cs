namespace SkyRoute.Application.DTOs
{
    public class ProviderFlightResponse
    {
        public string Provider { get; set; }
        public string Airline { get; set; }
        public string FlightCode { get; set; }
        public string OriginCode { get; set; }
        public string OriginName { get; set; }
        public string OriginCountry { get; set; }
        public string DestinationCode { get; set; }
        public string DestinationName { get; set; }
        public string DestinationCountry { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int DurationMinutes { get; set; }
        public string Cabin { get; set; }
        public int Passengers { get; set; }
        public decimal PricePerPassenger { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
