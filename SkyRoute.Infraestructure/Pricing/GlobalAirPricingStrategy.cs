using SkyRoute.Application.Interfaces;

namespace SkyRoute.Infraestructure.Pricing
{
    public class GlobalAirPricingStrategy : IProviderPricingStrategy
    {
        public decimal CalculatePrice(decimal baseFare)
        {
            return Math.Round(baseFare * 1.15m, 2, MidpointRounding.AwayFromZero);
        }
    }
}
