using Demand.Business.Abstract.OfferMediaService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.DemandMediaEntity;
using Demand.Domain.Entities.OfferMediaEntity;
using Demand.Infrastructure.DataAccess.Abstract.DemandMedia;
using Demand.Infrastructure.DataAccess.Abstract.OfferMedia;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.DemandMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Concrete.OfferMediaService
{
    public class OfferMediaService : IOfferMediaService
    {
        private readonly IOfferMediaRepository _offerMediaRepository;

        public OfferMediaService(IOfferMediaRepository offerMediaRepository)
        {
           _offerMediaRepository = offerMediaRepository;
        }
        public OfferMediaEntity AddOfferMedia(OfferMediaEntity offerMedia)
        {
            _offerMediaRepository.Add(offerMedia);
            return offerMedia;
        }

        public IDataResult<IList<OfferMediaEntity>> GetAll()
        {
            return new SuccessDataResult<IList<OfferMediaEntity>>(_offerMediaRepository.GetAll());
        }

        public IList<OfferMediaEntity> GetByDemandId(long id)
        {
            return _offerMediaRepository.GetList(x => x.OfferId == id);
        }
    }
}
