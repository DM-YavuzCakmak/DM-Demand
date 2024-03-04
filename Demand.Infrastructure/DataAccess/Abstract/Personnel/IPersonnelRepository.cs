using Demand.Core.DataAccess.EntityFramework;

namespace Demand.Infrastructure.DataAccess.Abstract.Personnel;

public interface IPersonnelRepository : IEntityRepository<Demand.Domain.Entities.Personnel.PersonnelEntity>
{
}
