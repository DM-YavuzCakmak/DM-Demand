using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.OfferRequestEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Abstract.OfferRequest
{
    public interface IOfferRequestRepository : IEntityRepository<OfferRequestEntity>
    {
    }
}
