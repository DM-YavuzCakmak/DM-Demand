using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Entities.PersonnelRole;
using Demand.Infrastructure.DataAccess.Abstract.Personnel;
using Demand.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRole
{
    public class PersonnelRoleRepository : EfEntityRepositoryBase<PersonnelRoleEntity, DemandContext>, IPersonnelRoleRepository
    {
    }
}
