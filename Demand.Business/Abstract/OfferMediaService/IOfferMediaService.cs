using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.OfferMediaEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Abstract.OfferMediaService
{
    public interface IOfferMediaService
    {
        IDataResult<IList<OfferMediaEntity>> GetAll();
        OfferMediaEntity AddOfferMedia(OfferMediaEntity offerMedia);
        IList<OfferMediaEntity> GetByDemandId(long id);
    }
}
