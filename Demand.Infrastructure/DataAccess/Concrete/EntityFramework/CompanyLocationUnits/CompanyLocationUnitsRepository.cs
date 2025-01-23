using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.CompanyLocation;
using Demand.Domain.Entities.CompanyLocationUnitsEntity;
using Demand.Infrastructure.DataAccess.Abstract.CompanyLocationUnits;
using Demand.Infrastructure.DataAccess.Abstract.ICompanyLocationRepository;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.CompanyLocationUnits
{
    public class CompanyLocationUnitsRepository : EfEntityRepositoryBase<CompanyLocationUnitsEntity, DemandContext>, ICompanyLocationUnitsRepository
    {
    }
}
