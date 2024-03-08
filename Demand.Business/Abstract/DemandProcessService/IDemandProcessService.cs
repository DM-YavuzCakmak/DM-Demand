using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DemandMediaEntity;
using Demand.Domain.Entities.DemandProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Abstract.DemandProcessService
{
    public interface IDemandProcessService
    {
        IDataResult<IList<DemandProcessEntity>> GetAll();
        DemandProcessEntity AddDemandProcess(DemandProcessEntity demandProcessEntity);
        DemandProcessEntity UpdateDemandProcess(DemandProcessEntity updatedDemandProcess);
        IDataResult<DemandProcessEntity> GetById(long id);


    }
}
