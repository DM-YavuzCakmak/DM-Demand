using Demand.Core.DataAccess.EntityFramework;

namespace Demand.Infrastructure.DataAccess.Abstract.IDemandRepository;

public interface IDemandRepository : IEntityRepository<Demand.Domain.Entities.Demand.DemandEntity>
{
}
