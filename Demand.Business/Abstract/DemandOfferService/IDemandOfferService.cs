using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.DemandOfferEntity;
using System.Linq.Expressions;

namespace Demand.Business.Abstract.DemandOfferService
{
    public interface IDemandOfferService
    {
        IDataResult<IList<DemandOfferEntity>> GetAll();
        DemandOfferEntity Add(DemandOfferEntity demandOffer);
        DemandOfferEntity Update(DemandOfferEntity demandOffer);
        IDataResult<DemandOfferEntity> GetById(long id);
        IDataResult<IList<DemandOfferEntity>> GetList(Expression<Func<DemandOfferEntity, bool>> filter);
    }
}
