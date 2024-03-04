using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.Demand;
using Demand.Infrastructure.DataAccess.Abstract.IDemandRepository;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Demand;

public class DemandRepository : EfEntityRepositoryBase<DemandEntity, DemandContext>, IDemandRepository
{
}
