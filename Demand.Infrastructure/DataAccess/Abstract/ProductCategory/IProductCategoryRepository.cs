using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.ProductCategoryEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Abstract.ProductCategory
{
    public interface IProductCategoryRepository: IEntityRepository<ProductCategoryEntity>
    {
    }
}
