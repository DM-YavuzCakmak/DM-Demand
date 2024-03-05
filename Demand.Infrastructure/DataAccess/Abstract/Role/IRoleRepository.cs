using Demand.Core.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Abstract.Role
{
    public interface IRoleRepository : IEntityRepository<Demand.Domain.Entities.Role.RoleEntity>
    {
    }
}
