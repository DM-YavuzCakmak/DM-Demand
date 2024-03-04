using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.Personnel;
using Demand.Infrastructure.DataAccess.Abstract.Personnel;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Personnel;

public class PersonnelRepository : EfEntityRepositoryBase<PersonnelEntity, DemandContext>, IPersonnelRepository
{
}
