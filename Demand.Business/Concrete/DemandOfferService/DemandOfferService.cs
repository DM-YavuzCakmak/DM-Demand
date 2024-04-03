using Demand.Business.Abstract.CurrencyTypeService;
using Demand.Business.Abstract.DemandOfferService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.CurrencyTypeEntity;
using Demand.Domain.Entities.DemandOfferEntity;
using Demand.Infrastructure.DataAccess.Abstract.DemandOffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Concrete.DemandOfferService
{
    public class DemandOfferService: IDemandOfferService
    {
        private readonly IDemandOfferRepository _demandOfferRepository;
        public DemandOfferService(IDemandOfferRepository demandOfferRepository)
        {
            _demandOfferRepository = demandOfferRepository;
        }

        public DemandOfferEntity Add(DemandOfferEntity demandOffer)
        {
            _demandOfferRepository.Add(demandOffer);
            return (demandOffer);
        }

        public DemandOfferEntity Update(DemandOfferEntity demandOffer)
        {
            _demandOfferRepository.Update(demandOffer);
            return (demandOffer);
        }

        public IDataResult<IList<DemandOfferEntity>> GetAll()
        {
            return new SuccessDataResult<IList<DemandOfferEntity>>(_demandOfferRepository.GetAll());
        }

        public IDataResult<DemandOfferEntity> GetById(long id)
        {
            return new SuccessDataResult<DemandOfferEntity>(_demandOfferRepository.Get(x => x.Id == id));
        }

        public IDataResult<IList<DemandOfferEntity>> GetList(Expression<Func<DemandOfferEntity, bool>> filter)
        {
            try
            {
                return new SuccessDataResult<IList<DemandOfferEntity>>(_demandOfferRepository.GetList(filter));

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
