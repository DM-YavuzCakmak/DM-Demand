using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.OfferRequestEntity;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Entities.ProductCategoryEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Abstract.ProductCategoryService
{
    public interface IProductCategoryService
    {
        IDataResult<List<ProductCategoryEntity>> GetList();
        IDataResult<IList<ProductCategoryEntity>> GetAll();

    }
}
