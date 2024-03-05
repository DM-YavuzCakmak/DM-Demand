using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.DemandMediaEntity;
using Demand.Domain.Entities.DemandProcess;
using Demand.Infrastructure.DataAccess.Abstract.DemandMedia;
using Demand.Infrastructure.DataAccess.Abstract.DemandProcess;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.DemandProcess
{
    public class DemandProcessRepository: EfEntityRepositoryBase<DemandProcessEntity, DemandContext>,IDemandProcessRepository
    {
    }
}
