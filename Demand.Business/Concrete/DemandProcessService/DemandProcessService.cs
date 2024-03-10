using Demand.Business.Abstract.DemandProcessService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.DemandProcess;
using Demand.Infrastructure.DataAccess.Abstract.DemandProcess;
using System.Linq.Expressions;

namespace Demand.Business.Concrete.DemandProcessService
{
    public class DemandProcessService : IDemandProcessService
    {
        private readonly IDemandProcessRepository _demandProcessRepository;
        public DemandProcessService(IDemandProcessRepository demandProcessRepository)
        {
            _demandProcessRepository = demandProcessRepository;
        }
        public DemandProcessEntity AddDemandProcess(DemandProcessEntity demandProcessEntity)
        {
            _demandProcessRepository.Add(demandProcessEntity);
            return demandProcessEntity;
        }
        public IDataResult<IList<DemandProcessEntity>> GetAll()
        {
            return new SuccessDataResult<IList<DemandProcessEntity>>(_demandProcessRepository.GetAll());
        }
        public IDataResult<DemandProcessEntity> GetById(long id)
        {
            return new SuccessDataResult<DemandProcessEntity>(_demandProcessRepository.Get(x => x.Id == id));
        }

        public IDataResult<IList<DemandProcessEntity>> GetList(Expression<Func<DemandProcessEntity, bool>> filter)
        {
            return new SuccessDataResult<IList<DemandProcessEntity>>(_demandProcessRepository.GetList(filter));
        }

        public DemandProcessEntity UpdateDemandProcess(DemandProcessEntity updatedDemandProcess)
        {
            _demandProcessRepository.Update(updatedDemandProcess);
            return updatedDemandProcess;
        }
    }
}
