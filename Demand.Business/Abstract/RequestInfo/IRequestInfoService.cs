using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.RequestInfoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Abstract.RequestInfo
{
    public interface IRequestInfoService
    {
        IDataResult<IList<RequestInfoEntity>> GetAll();
        RequestInfoEntity Add(RequestInfoEntity requestInfo);
        RequestInfoEntity Update(RequestInfoEntity requestInfo);
        IDataResult<RequestInfoEntity> GetById(long id);
        IDataResult<IList<RequestInfoEntity>> GetList(Expression<Func<RequestInfoEntity, bool>> filter);
        IDataResult<RequestInfoEntity> GetByDemandId(long demandId);

    }
}
