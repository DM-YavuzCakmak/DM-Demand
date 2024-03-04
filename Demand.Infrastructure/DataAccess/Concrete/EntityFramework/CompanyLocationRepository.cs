using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.CompanyLocation;
using Demand.Infrastructure.DataAccess.Abstract;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework
{
    public class CompanyLocationRepository : EfEntityRepositoryBase<CompanyLocationEntity, DemandContext>, ICompanyLocationRepository
    {
    }
}
