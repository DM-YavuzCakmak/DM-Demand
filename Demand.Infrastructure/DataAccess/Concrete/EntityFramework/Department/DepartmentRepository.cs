using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.DemandProcess;
using Demand.Domain.Entities.DepartmentEntity;
using Demand.Infrastructure.DataAccess.Abstract.DemandProcess;
using Demand.Infrastructure.DataAccess.Abstract.Department;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Department
{
    public class DepartmentRepository : EfEntityRepositoryBase<DepartmentEntity, DemandContext>, IDepartmentRepository
    {
    }
}
