using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.PersonnelDepartmentEntity;
using Demand.Infrastructure.DataAccess.Abstract.PersonnelDepartment;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelDepartment
{
    public class PersonnelDepartmentRepository : EfEntityRepositoryBase<PersonnelDepartmentEntity, DemandContext>, IPersonnelDepartmentRepository
    {
    }
}
