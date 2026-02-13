using Demand.Business.Abstract.PersonnelDepartmentService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.PersonnelDepartmentEntity;
using Demand.Infrastructure.DataAccess.Abstract.PersonnelDepartment;
using System.Linq.Expressions;

namespace Demand.Business.Concrete.PersonnelDepartmentService
{
    public class PersonnelDepartmentService : IPersonnelDepartmentService
    {
        private readonly IPersonnelDepartmentRepository _personnelDepartmentRepository;

        public PersonnelDepartmentService(IPersonnelDepartmentRepository personnelDepartmentRepository)
        {
            _personnelDepartmentRepository = personnelDepartmentRepository;
        }

        public IDataResult<PersonnelDepartmentEntity> GetById(long id)
        {
            return new SuccessDataResult<PersonnelDepartmentEntity>(_personnelDepartmentRepository.Get(x => x.Id == id));
        }

        public IDataResult<IList<PersonnelDepartmentEntity>> GetList(Expression<Func<PersonnelDepartmentEntity, bool>> filter)
        {
            return new SuccessDataResult<IList<PersonnelDepartmentEntity>>(_personnelDepartmentRepository.GetList(filter));
        }
    }
}
