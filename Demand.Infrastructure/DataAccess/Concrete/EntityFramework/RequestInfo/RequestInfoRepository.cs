using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.RequestInfoEntity;
using Demand.Infrastructure.DataAccess.Abstract.RequestInfo;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.RequestInfo
{
    public class RequestInfoRepository:EfEntityRepositoryBase<RequestInfoEntity, DemandContext>, IRequestInfoRepository
    {
    }
}
