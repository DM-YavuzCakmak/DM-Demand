using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DemandMediaEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Abstract.DemandMediaService
{
    public interface IDemandMediaService
    {
        IDataResult<IList<DemandMediaEntity>> GetAll();
        DemandMediaEntity AddDemandMedia(DemandMediaEntity demandMedia);
        IList<DemandMediaEntity> GetByDemandId(long id);
    }
}
