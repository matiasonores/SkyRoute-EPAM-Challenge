namespace SkyRoute.Application.Interfaces
{
    public interface IProviderPricingStrategy
    {
        decimal CalculatePrice(decimal baseFare);
    }
}
