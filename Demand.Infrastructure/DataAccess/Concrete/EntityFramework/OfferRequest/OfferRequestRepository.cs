using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.DepartmentEntity;
using Demand.Domain.Entities.OfferRequestEntity;
using Demand.Infrastructure.DataAccess.Abstract.Department;
using Demand.Infrastructure.DataAccess.Abstract.OfferRequest;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.OfferRequest
{
    public class OfferRequestRepository: EfEntityRepositoryBase<OfferRequestEntity, DemandContext>, IOfferRequestRepository
    {
    }
}
