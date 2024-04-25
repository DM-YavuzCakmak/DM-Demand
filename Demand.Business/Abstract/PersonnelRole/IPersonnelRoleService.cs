using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.OfferRequestEntity;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Entities.PersonnelRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Abstract.PersonnelRole
{
    public interface IPersonnelRoleService
    {
        IDataResult<IList<PersonnelRoleEntity>> GetList(Expression<Func<PersonnelRoleEntity, bool>> filter);

        IDataResult<PersonnelRoleEntity> GetById(long id);
    }
}
