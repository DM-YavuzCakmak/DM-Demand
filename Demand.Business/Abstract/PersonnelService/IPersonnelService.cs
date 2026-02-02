using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Personnel;
using System.Linq.Expressions;

namespace Demand.Business.Abstract.PersonnelService
{
    public interface IPersonnelService
    {
        IDataResult<List<PersonnelEntity>> GetList(Expression<Func<PersonnelEntity, bool>> filter = null);
        IDataResult<PersonnelEntity> GetById(long id);
        IDataResult<PersonnelEntity> GetByEmail(string email);
        List<PersonnelEntity> GetAllParentList(long id);
    }
}
