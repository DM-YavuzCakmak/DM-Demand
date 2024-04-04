using Demand.Business.Abstract.ProductCategoryService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.DemandOfferEntity;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Entities.ProductCategoryEntity;
using Demand.Infrastructure.DataAccess.Abstract.ProductCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Concrete.ProductCategoryService
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        public ProductCategoryService(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public IDataResult<IList<ProductCategoryEntity>> GetAll()
        {
            return new SuccessDataResult<IList<ProductCategoryEntity>>(_productCategoryRepository.GetAll());
        }

        public IDataResult<List<ProductCategoryEntity>> GetList()
        {
            return new SuccessDataResult<List<ProductCategoryEntity>>(_productCategoryRepository.GetList().ToList());
        }
    }
}
