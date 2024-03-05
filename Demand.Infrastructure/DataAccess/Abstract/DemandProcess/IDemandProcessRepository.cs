using Demand.Core.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Abstract.DemandProcess
{
    public interface IDemandProcessRepository : IEntityRepository<Demand.Domain.Entities.DemandProcess.DemandProcessEntity>
    {
    }
}
