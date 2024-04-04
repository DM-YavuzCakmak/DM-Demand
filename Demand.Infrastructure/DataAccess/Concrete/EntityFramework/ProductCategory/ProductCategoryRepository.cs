using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.PersonnelRole;
using Demand.Domain.Entities.ProductCategoryEntity;
using Demand.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Demand.Infrastructure.DataAccess.Abstract.ProductCategory;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.ProductCategory
{
    public class ProductCategoryRepository : EfEntityRepositoryBase<ProductCategoryEntity, DemandContext>, IProductCategoryRepository
    {
    }
}
