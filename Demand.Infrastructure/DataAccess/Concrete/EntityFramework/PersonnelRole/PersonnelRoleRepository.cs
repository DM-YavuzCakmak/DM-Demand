using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.PersonnelRole;
using Demand.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRole
{
    public class PersonnelRoleRepository : EfEntityRepositoryBase<PersonnelRoleEntity, DemandContext>, IPersonnelRoleRepository
    {
    }
}
