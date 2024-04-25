using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Entities.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Abstract.RoleService
{
    public interface IRoleService
    {
        IDataResult<RoleEntity> GetById(long id);

    }
}
