using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.Company;

namespace Demand.Infrastructure.DataAccess.Abstract.ICompanyRepository;

public interface ICompanyRepository : IEntityRepository<Company>
{

}
