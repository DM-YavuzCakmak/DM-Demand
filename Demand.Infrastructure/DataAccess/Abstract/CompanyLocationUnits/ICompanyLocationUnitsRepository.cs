using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.CompanyLocationUnitsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Abstract.CompanyLocationUnits
{
  
    public interface ICompanyLocationUnitsRepository : IEntityRepository<CompanyLocationUnitsEntity>
    {
    }
}
