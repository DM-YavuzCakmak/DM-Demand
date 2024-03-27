using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.ApprovedSupplierEntity;
using Demand.Domain.Entities.Company;
using Demand.Infrastructure.DataAccess.Abstract.ApprovedSuplier;
using Demand.Infrastructure.DataAccess.Abstract.ICompanyRepository;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.ApprovedSupplier
{
    public class ApprovedSupplierRepository : EfEntityRepositoryBase<ApprovedSupplierEntity, DemandContext>, IApprovedSupplierRepository
    {
    }
}
