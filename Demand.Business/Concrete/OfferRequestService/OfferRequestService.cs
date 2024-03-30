using Demand.Business.Abstract.OfferRequestService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DepartmentEntity;
using Demand.Domain.Entities.OfferRequestEntity;
using Demand.Infrastructure.DataAccess.Abstract.OfferRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Concrete.OfferRequestService
{
    public class OfferRequestService : IOfferRequestService
    {
        private readonly IOfferRequestRepository _offerRequestRepository;
        public OfferRequestService(IOfferRequestRepository offerRequestRepository)
        {
            _offerRequestRepository = offerRequestRepository;
        }
        public OfferRequestEntity Add(OfferRequestEntity offerRequest)
        {
            _offerRequestRepository.Add(offerRequest);
            return offerRequest;
        }

        public IDataResult<IList<OfferRequestEntity>> GetAll()
        {
            return new SuccessDataResult<IList<OfferRequestEntity>>(_offerRequestRepository.GetAll());
        }

        public IDataResult<OfferRequestEntity> GetById(long id)
        {
            return new SuccessDataResult<OfferRequestEntity>(_offerRequestRepository.Get(x => x.Id == id));
        }

        public IDataResult<IList<OfferRequestEntity>> GetList(Expression<Func<OfferRequestEntity, bool>> filter)
        {
            return new SuccessDataResult<IList<OfferRequestEntity>>(_offerRequestRepository.GetList(filter));
        }

        public OfferRequestEntity Update(OfferRequestEntity offerRequest)
        {
            _offerRequestRepository.Update(offerRequest);
            return offerRequest;
        }
    }
}
