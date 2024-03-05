using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.DemandMediaEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Abstract.DemandMedia
{
    public interface IDemandMediaRepository :IEntityRepository<Demand.Domain.Entities.DemandMediaEntity.DemandMediaEntity>
    {
    }
}
