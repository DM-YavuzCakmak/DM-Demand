using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.DemandOfferEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Abstract.DemandOffer
{
    public interface IDemandOfferRepository: IEntityRepository<DemandOfferEntity>
    {
    }
}
