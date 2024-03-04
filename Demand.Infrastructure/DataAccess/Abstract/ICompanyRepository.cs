using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.Company;

namespace Demand.Infrastructure.DataAccess.Abstract;

public interface ICompanyRepository : IEntityRepository<CompanyEntity>
{

}
