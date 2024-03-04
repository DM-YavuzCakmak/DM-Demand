using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.CompanyLocation;
using Demand.Infrastructure.DataAccess.Abstract.ICompanyLocationRepository;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.CompanyLocationRepository
{
    public class CompanyLocationRepository : EfEntityRepositoryBase<CompanyLocation, DemandContext>, ICompanyLocationRepository
    {
    }
}
