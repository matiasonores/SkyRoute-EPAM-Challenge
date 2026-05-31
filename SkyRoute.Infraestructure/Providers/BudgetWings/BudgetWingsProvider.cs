using Microsoft.Extensions.Options;
using SkyRoute.Application.DTOs;
using SkyRoute.Application.Interfaces;
using SkyRoute.Infraestructure.Pricing;

namespace SkyRoute.Infraestructure.Providers.BudgetWings
{
    public class BudgetWingsProvider : BaseFlightProvider
    {
        public BudgetWingsProvider(IOptionsMonitor<FlightProviderSettings> options, IProviderReservationRepository reservationRepository)
            : base(options.Get("BudgetWings"), reservationRepository, new BudgetWingsPricingStrategy())
        {

        }
    }
}
