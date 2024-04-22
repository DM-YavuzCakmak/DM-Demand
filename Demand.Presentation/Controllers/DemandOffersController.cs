using Demand.Business.Abstract.DemandMediaService;
using Demand.Business.Abstract.DemandOfferService;
using Demand.Business.Abstract.DemandService;
using Demand.Business.Abstract.Provider;
using Demand.Business.Abstract.RequestInfo;
using Demand.Business.Concrete.DemandMediaService;
using Demand.Business.Concrete.DemandService;
using Demand.Core.Attribute;
using Demand.Domain.Entities.DemandOfferEntity;
using Demand.Domain.Entities.ProviderEntity;
using Demand.Domain.Entities.RequestInfoEntity;
using Demand.Domain.ViewModels;
using Kep.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Demand.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserToken]
    public class DemandOffersController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDemandOfferService _demandOfferService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProviderService _providerService;
        private readonly IRequestInfoService _requestInfoService;

        public DemandOffersController(ILogger<HomeController> logger, IDemandOfferService demandOfferService, IWebHostEnvironment webHostEnvironment, IProviderService providerService, IRequestInfoService requestInfoService)
        {
            _logger = logger;
            _demandOfferService = demandOfferService;
            _webHostEnvironment = webHostEnvironment;
            _providerService = providerService;
            _requestInfoService = requestInfoService;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GetDemandOfferData(long demandId)
        {
            List<DemandOfferEntity> demandOfferEntities = _demandOfferService.GetList(x => x.DemandId == demandId).Data.ToList();
            List<UpdateDemandViewModel> demandOfferData = new List<UpdateDemandViewModel>();

            foreach (var demandOfferEntity in demandOfferEntities)
            {
                for (int i = 1; i <= 3; i++)
                {
                    // ViewModel for Offer i
                    UpdateDemandViewModel viewModel = new UpdateDemandViewModel
                    {
                        DemandId = demandOfferEntity.DemandId,
                    };

                    ProviderEntity provider = _providerService.GetById((long)demandOfferEntity.SupplierId).Data;
                    //RequestInfoEntity requestInfo = _requestInfoService.GetById(demandOfferEntity.RequestInfoId).Data;

                    //string offerPrefix = $"Offer{i}";
                    //viewModel.GetType().GetProperty($"{offerPrefix}CompanyName").SetValue(viewModel, demandOfferEntity.SupplierName);
                    //viewModel.GetType().GetProperty($"{offerPrefix}CompanyId").SetValue(viewModel, demandOfferEntity.SupplierId);
                    //viewModel.GetType().GetProperty($"{offerPrefix}CompanyPhone").SetValue(viewModel, demandOfferEntity.SupplierPhone);
                    //viewModel.GetType().GetProperty($"{offerPrefix}CompanyAddress").SetValue(viewModel, provider.Address);
                    //viewModel.GetType().GetProperty($"{offerPrefix}CurrencyType").SetValue(viewModel, demandOfferEntity.CurrencyTypeId);
                    //viewModel.GetType().GetProperty($"{offerPrefix}Amount").SetValue(viewModel, requestInfo.Quantity);
                    //viewModel.GetType().GetProperty($"{offerPrefix}Material").SetValue(viewModel, requestInfo.ProductName);
                    ////viewModel.GetType().GetProperty($"{offerPrefix}Price").SetValue(viewModel, requestInfo.Price);
                    //viewModel.GetType().GetProperty($"{offerPrefix}TotalPrice").SetValue(viewModel, demandOfferEntity.TotalPrice);

                    demandOfferData.Add(viewModel);
                }
            }

            return Json(demandOfferData);
        }


    }
}
