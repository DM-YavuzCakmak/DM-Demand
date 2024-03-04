using Demand.Core.Entities;

namespace Demand.Domain.Entities
{
    public class BaseEntity : IEntity
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
    }
}
