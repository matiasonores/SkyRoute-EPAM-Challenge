using SkyRoute.Domain.Entities.BaseEntities;

namespace SkyRoute.Domain.Entities
{
    public class Airport : BaseEntity<int>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string? City { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
