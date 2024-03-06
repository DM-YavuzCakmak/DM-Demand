using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Demand;

namespace Demand.Business.Abstract.DemandService;

public interface IDemandService
{
    IDataResult<IList<DemandEntity>> GetAll();
}
