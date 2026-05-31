namespace SkyRoute.Infraestructure.Providers
{
    using System.Text.Json;
    using SkyRoute.Application.DTOs;
    using SkyRoute.Application.Interfaces;

    public abstract class BaseFlightProvider : IFlightProvider
    {

        protected readonly FlightProviderSettings Settings;
        protected readonly IProviderReservationRepository ReservationRepository;
        protected readonly IProviderPricingStrategy PricingStrategy;

        public string Provider => Settings.ProviderName;

        protected BaseFlightProvider(FlightProviderSettings settings, IProviderReservationRepository reservationRepository, IProviderPricingStrategy pricingStrategy)
        {
            Settings = settings;
            ReservationRepository = reservationRepository;
            PricingStrategy = pricingStrategy;
        }

        public virtual async Task<FlightProviderSearchResponse>SearchFlightsAsync(FlightSearchRequest request, CancellationToken cancellationToken = default)
        {
            var generatedFlights = FlightGenerator.GenerateFlights(request,Settings,PricingStrategy);
            var availableFlights = new List<ProviderFlightResponse>();

            foreach (var flight in generatedFlights)
            {
                var reserved = await ReservationRepository.IsReservedAsync(Settings.ProviderName,flight.FlightCode,cancellationToken);

                if (!reserved)
                {
                    availableFlights.Add(flight);
                }
            }

            return new FlightProviderSearchResponse
            {
                Flights = availableFlights
            };
        }

        public virtual async Task BookFlightAsync(string flightNumber,CancellationToken cancellationToken = default)
        {
            await ReservationRepository.BookFlightAsync(Settings.ProviderName,flightNumber,cancellationToken);
        }
    }
}
