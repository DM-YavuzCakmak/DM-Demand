using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.DemandProcess;
using Demand.Infrastructure.DataAccess.Abstract.DemandProcess;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.DemandProcess;

public class DemandProcessRepository : EfEntityRepositoryBase<DemandProcessEntity, DemandContext>, IDemandProcessRepository
{
}
