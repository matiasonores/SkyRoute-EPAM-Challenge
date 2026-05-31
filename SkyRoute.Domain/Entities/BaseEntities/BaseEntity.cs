namespace SkyRoute.Domain.Entities.BaseEntities
{
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; set; }
    }
}
