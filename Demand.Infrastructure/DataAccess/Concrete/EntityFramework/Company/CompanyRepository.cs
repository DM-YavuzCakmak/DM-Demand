using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.Company;
using Demand.Infrastructure.DataAccess.Abstract.ICompanyRepository;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.CompanyRepository
{
    public class CompanyRepository : EfEntityRepositoryBase<Company, DemandContext>, ICompanyRepository
    {
    }
}
