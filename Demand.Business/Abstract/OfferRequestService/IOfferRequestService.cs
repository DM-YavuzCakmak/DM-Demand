using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.OfferRequestEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Abstract.OfferRequestService
{
    public interface IOfferRequestService
    {
        IDataResult<IList<OfferRequestEntity>> GetAll();
        OfferRequestEntity Add(OfferRequestEntity offerRequest);
        OfferRequestEntity Update(OfferRequestEntity offerRequest);
        IDataResult<OfferRequestEntity> GetById(long id);
        IDataResult<IList<OfferRequestEntity>> GetList(Expression<Func<OfferRequestEntity, bool>> filter);
    }
}
