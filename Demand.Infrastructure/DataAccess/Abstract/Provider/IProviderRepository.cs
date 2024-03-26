using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.ProviderEntity;


namespace Demand.Infrastructure.DataAccess.Abstract.Provider
{
    public interface IProviderRepository: IEntityRepository<ProviderEntity>
    {
    }
}
