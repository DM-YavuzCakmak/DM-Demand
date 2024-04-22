using Demand.Business.Abstract.RequestInfo;
using Demand.Business.Concrete.RequestInfo;
using Demand.Core.Attribute;
using Demand.Domain.Entities.RequestInfoEntity;
using Demand.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Demand.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserToken]

    public class RequestInfoController : Controller
    {
        private readonly IRequestInfoService _requestInfoService;

        public RequestInfoController(IRequestInfoService requestInfoService)
        {
            _requestInfoService = requestInfoService;
        }

        [HttpPost("RequestInfo")]
        public IActionResult RequestInfo([FromBody] OfferDetailViewModel offerDetailViewModel)
        {
            var offerRequestViewModel = new OfferRequestViewModel
            {
                DemandId = (long)offerDetailViewModel.DemandId,
                ProductCategoryId = 1, // Örnek olarak sabit bir değer atandı
                ProductSubCategoryId = 1, // Örnek olarak sabit bir değer atandı
                ProductName = "Örnek Ürün", // Örnek olarak sabit bir değer atandı
                Quantity = 10, // Örnek olarak sabit bir değer atandı
                Unit = "Adet", // Örnek olarak sabit bir değer atandı
                Price = 0, // Varsayılan değer olarak 0 atandı
                TotalPrice = 0 // Varsayılan değer olarak 0 atandı
            };

            return View(offerRequestViewModel);
        }
    }

}
