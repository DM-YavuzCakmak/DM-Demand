using Demand.Core.DataAccess.EntityFramework;

namespace Demand.Infrastructure.DataAccess.Abstract.PersonnelDepartment
{
    public interface IPersonnelDepartmentRepository : IEntityRepository<Demand.Domain.Entities.PersonnelDepartmentEntity.PersonnelDepartmentEntity>
    {
    }
}
