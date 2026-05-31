using SkyRoute.Domain.Entities.BaseEntities;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Domain.Entities
{
    public class Passenger : BaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string? NationalId { get; set; }
        public string? PassportNumber { get; set; }
    }
}
