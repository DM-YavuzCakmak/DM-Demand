using Demand.Business.Abstract.RoleService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Entities.Role;
using Demand.Infrastructure.DataAccess.Abstract.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Concrete.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public IDataResult<RoleEntity> GetById(long id)
        {
            return new SuccessDataResult<RoleEntity>(_roleRepository.Get(x => x.Id == id));

        }
    }
}
