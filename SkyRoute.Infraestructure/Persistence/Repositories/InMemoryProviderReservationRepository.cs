using SkyRoute.Application.Interfaces;

namespace SkyRoute.Infraestructure.Persistence.Repositories
{
    public class InMemoryProviderReservationRepository : IProviderReservationRepository
    {
        private readonly HashSet<string> _reservedFlights = [];

        public Task BookFlightAsync(string providerName, string flightNumber, CancellationToken cancellationToken = default)
        {
            _reservedFlights.Add($"{providerName}:{flightNumber}");

            return Task.CompletedTask;
        }

        public Task<bool> IsReservedAsync(string providerName, string flightNumber, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_reservedFlights.Contains($"{providerName}:{flightNumber}"));
        }
    }
}
