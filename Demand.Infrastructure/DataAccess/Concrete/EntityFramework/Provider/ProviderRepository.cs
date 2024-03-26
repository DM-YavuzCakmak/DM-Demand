using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.ProviderEntity;
using Demand.Infrastructure.DataAccess.Abstract.Provider;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Provider
{
    public class ProviderRepository: EfEntityRepositoryBase<ProviderEntity, DemandContext>, IProviderRepository
    {
    }
}
