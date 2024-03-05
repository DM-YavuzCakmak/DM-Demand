using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.PersonnelRole;
using Demand.Domain.Entities.Role;
using Demand.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Demand.Infrastructure.DataAccess.Abstract.Role;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Role
{
    public class RoleRepository : EfEntityRepositoryBase<RoleEntity, DemandContext>, IRoleRepository
    {
    }
}
