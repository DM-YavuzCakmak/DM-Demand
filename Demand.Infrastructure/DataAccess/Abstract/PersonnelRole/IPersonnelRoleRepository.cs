using Demand.Core.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Abstract.PersonnelRole
{
    public interface IPersonnelRoleRepository : IEntityRepository<Demand.Domain.Entities.PersonnelRole.PersonnelRoleEntity>
    {
    }
}
