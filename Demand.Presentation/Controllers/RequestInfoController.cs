using Demand.Business.Abstract.RequestInfo;
using Demand.Business.Concrete.RequestInfo;
using Demand.Domain.Entities.RequestInfoEntity;
using Demand.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Demand.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestInfoController : Controller
    {
        private readonly IRequestInfoService _requestInfoService;

        public RequestInfoController(IRequestInfoService requestInfoService)
        {
            _requestInfoService = requestInfoService;
        }

        [HttpPost("RequestInfo")]
        public IActionResult RequestInfo([FromBody] RQ rQ)
        {
            RequestInfoOfferViewModel requestInfoOfferViewModel = new RequestInfoOfferViewModel();
            RequestInfoEntity requestInfoEntity = _requestInfoService.GetById(rQ.RequestInfoId).Data;
            if (requestInfoEntity != null)
            {
                requestInfoOfferViewModel.Unit = requestInfoEntity.Unit;
                requestInfoOfferViewModel.Quantity = requestInfoEntity.Quantity;
            }
            return View(requestInfoOfferViewModel);
        }
    }

    public class RQ
    {
        public long RequestInfoId { get; set; }
    }
}
