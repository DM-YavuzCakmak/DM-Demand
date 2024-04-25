using Demand.Business.Abstract.PersonnelRole;
using Demand.Business.Abstract.PersonnelService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Entities.PersonnelRole;
using Demand.Domain.Entities.RequestInfoEntity;
using Demand.Infrastructure.DataAccess.Abstract.PersonnelRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Concrete.PersonnelRoleService
{
    public class PersonnelRoleService : IPersonnelRoleService
    {
        private readonly IPersonnelRoleRepository  _personnelRoleRepository;

        public PersonnelRoleService(IPersonnelRoleRepository personnelRoleRepository)
        {
            _personnelRoleRepository = personnelRoleRepository;
        }
        public IDataResult<PersonnelRoleEntity> GetById(long id)
        {
            return new SuccessDataResult<PersonnelRoleEntity>(_personnelRoleRepository.Get(x => x.Id == id));
        }

        public IDataResult<IList<PersonnelRoleEntity>> GetList(Expression<Func<PersonnelRoleEntity, bool>> filter)
        {
            return new SuccessDataResult<IList<PersonnelRoleEntity>>(_personnelRoleRepository.GetList(filter));
        }
    }
}
