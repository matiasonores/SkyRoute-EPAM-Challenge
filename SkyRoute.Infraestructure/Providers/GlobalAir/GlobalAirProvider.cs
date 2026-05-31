using Microsoft.Extensions.Options;
using SkyRoute.Application.DTOs;
using SkyRoute.Application.Interfaces;
using SkyRoute.Infraestructure.Pricing;

namespace SkyRoute.Infraestructure.Providers.GlobalAir
{
    public class GlobalAirProvider: BaseFlightProvider
    {
        public GlobalAirProvider(IOptionsMonitor<FlightProviderSettings> options, IProviderReservationRepository reservationRepository)
            : base(options.Get("GlobalAir"), reservationRepository, new GlobalAirPricingStrategy())
        {

        }
    }
}
