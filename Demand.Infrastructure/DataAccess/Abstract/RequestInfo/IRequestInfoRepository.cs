using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.RequestInfoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Abstract.RequestInfo
{
    public interface IRequestInfoRepository: IEntityRepository<RequestInfoEntity>
    {
    }
}
