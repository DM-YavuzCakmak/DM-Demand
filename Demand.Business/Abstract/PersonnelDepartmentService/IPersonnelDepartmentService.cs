using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.PersonnelDepartmentEntity;
using System.Linq.Expressions;

namespace Demand.Business.Abstract.PersonnelDepartmentService
{
    public interface IPersonnelDepartmentService
    {
        IDataResult<IList<PersonnelDepartmentEntity>> GetList(Expression<Func<PersonnelDepartmentEntity, bool>> filter);

        IDataResult<PersonnelDepartmentEntity> GetById(long id);
    }
}
