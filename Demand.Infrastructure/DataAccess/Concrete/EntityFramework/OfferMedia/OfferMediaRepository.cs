using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.DemandMediaEntity;
using Demand.Domain.Entities.OfferMediaEntity;
using Demand.Infrastructure.DataAccess.Abstract.DemandMedia;
using Demand.Infrastructure.DataAccess.Abstract.OfferMedia;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.OfferMedia
{
    public class OfferMediaRepository : EfEntityRepositoryBase<OfferMediaEntity, DemandContext>, IOfferMediaRepository
    {
    }
}
