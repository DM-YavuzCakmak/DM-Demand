using Demand.Business.Abstract.DemandMediaService;
using Demand.Business.Abstract.DemandProcessService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DemandMediaEntity;
using Demand.Domain.Entities.DemandProcess;
using Demand.Infrastructure.DataAccess.Abstract.DemandMedia;
using Demand.Infrastructure.DataAccess.Abstract.DemandProcess;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.DemandMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DemandProcessEntity UpdateDemandProcess(DemandProcessEntity updatedDemandProcess)
        {
            _demandProcessRepository.Update(updatedDemandProcess);
            return updatedDemandProcess;
        }
    }
}
