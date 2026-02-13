using Demand.Core.DataAccess.EntityFramework;

namespace Demand.Infrastructure.DataAccess.Abstract.PersonnelRole
{
    public interface IPersonnelRoleRepository : IEntityRepository<Demand.Domain.Entities.PersonnelRole.PersonnelRoleEntity>
    {
    }
}
