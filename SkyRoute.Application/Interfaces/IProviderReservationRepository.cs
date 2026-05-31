namespace SkyRoute.Application.Interfaces
{
    public interface IProviderReservationRepository
    {
        Task BookFlightAsync(string providerName, string flightNumber, CancellationToken cancellationToken = default);
        Task<bool> IsReservedAsync(string providerName, string flightNumber, CancellationToken cancellationToken = default);
    }
}
