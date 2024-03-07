using Demand.Business.Abstract.DemandService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.Demand;
using Demand.Infrastructure.DataAccess.Abstract.IDemandRepository;

namespace Demand.Business.Concrete.DemandService
{
    public class DemandService : IDemandService
    {
        private readonly IDemandRepository _demandRepository;
        public DemandService(IDemandRepository demandRepository)
        {
            _demandRepository = demandRepository;
        }

        public DemandEntity AddDemand(DemandEntity demand)
        {
           _demandRepository.Add(demand);
            return demand;
        }

        public IDataResult<IList<DemandEntity>> GetAll()
        {
            return new SuccessDataResult<IList<DemandEntity>>(_demandRepository.GetAll());
        }


        public IDataResult<DemandEntity> GetById(long id)
        {
            return new SuccessDataResult<DemandEntity>(_demandRepository.Get(x => x.Id == id));
        }
    }
}
