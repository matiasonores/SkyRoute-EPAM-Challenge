using SkyRoute.Application.Interfaces;
using SkyRoute.Domain.Entities;

public interface IPassengerRepository : IGenericRepository<Passenger, Guid>
{
    Task<Passenger?> GetByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default);
    Task<Passenger?> GetByPassportNumberAsync(string passportNumber, CancellationToken cancellationToken = default);
}