using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.ProviderEntity;
using System.Linq.Expressions;

namespace Demand.Business.Abstract.Provider
{
    public interface IProviderService
    {
        IDataResult<IList<ProviderEntity>> GetAll();
        ProviderEntity Add(ProviderEntity provider);
        ProviderEntity Update(ProviderEntity provider);
        IDataResult<ProviderEntity> GetById(long id);
        IDataResult<IList<ProviderEntity>> GetList(Expression<Func<ProviderEntity, bool>> filter);
    }
}
