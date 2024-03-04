using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.Company;
using Demand.Infrastructure.DataAccess.Abstract;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework
{
    public class CompanyRepository : EfEntityRepositoryBase<Company, DemandContext>, ICompanyRepository
    {
    }
}
