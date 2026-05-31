using SkyRoute.Application.Interfaces;

namespace SkyRoute.Infraestructure.Pricing
{
    public class BudgetWingsPricingStrategy : IProviderPricingStrategy
    {
        private const decimal MinimumPrice = 29.99m;

        public decimal CalculatePrice(decimal baseFare)
        {
            decimal discountRate = 0.10m;

            var discountedPrice = baseFare * (1 - discountRate);

            var roundedPrice = Math.Round(discountedPrice, 2,MidpointRounding.AwayFromZero);

            return Math.Max(roundedPrice, MinimumPrice);
        }
    }
}
