using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.Demand;
using System.Linq.Expressions;

namespace Demand.Business.Abstract.DemandService;

public interface IDemandService
{
    IDataResult<IList<DemandEntity>> GetAll();
    DemandEntity AddDemand(DemandEntity demand);
    DemandEntity Update(DemandEntity demand);
    IDataResult<DemandEntity> GetById(long id);
    IDataResult<IList<DemandEntity>> GetList(Expression<Func<DemandEntity, bool>> filter);


}
