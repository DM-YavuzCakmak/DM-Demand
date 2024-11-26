using Demand.Business.Abstract.CompanyLocation;
using Demand.Business.Abstract.CompanyService;
using Demand.Business.Abstract.CurrencyTypeService;
using Demand.Business.Abstract.DemandMediaService;
using Demand.Business.Abstract.DemandOfferService;
using Demand.Business.Abstract.DemandProcessService;
using Demand.Business.Abstract.DemandService;
using Demand.Business.Abstract.Department;
using Demand.Business.Abstract.OfferMediaService;
using Demand.Business.Abstract.OfferRequestService;
using Demand.Business.Abstract.PersonnelRole;
using Demand.Business.Abstract.PersonnelService;
using Demand.Business.Abstract.ProductCategoryService;
using Demand.Business.Abstract.Provider;
using Demand.Business.Abstract.RequestInfo;
using Demand.Business.Abstract.RoleService;
using Demand.Business.Concrete.ProductCategoryService;
using Demand.Core.Attribute;
using Demand.Core.DatabaseConnection.NebimConnection;
using Demand.Core.Utilities.Email;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.CompanyLocation;
using Demand.Domain.Entities.CurrencyTypeEntity;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DemandMediaEntity;
using Demand.Domain.Entities.DemandOfferEntity;
using Demand.Domain.Entities.DemandProcess;
using Demand.Domain.Entities.DepartmentEntity;
using Demand.Domain.Entities.OfferMediaEntity;
using Demand.Domain.Entities.OfferRequestEntity;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Entities.PersonnelRole;
using Demand.Domain.Entities.ProductCategoryEntity;
using Demand.Domain.Entities.ProviderEntity;
using Demand.Domain.Entities.RequestInfoEntity;
using Demand.Domain.Entities.Role;
using Demand.Domain.Enums;
using Demand.Domain.ViewModels;
using Demand.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Kep.Helpers.Extensions;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;


namespace Demand.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserToken]
    public class DemandsController : Controller
    {
        private readonly IDemandService _demandService;
        private readonly ILogger<HomeController> _logger;
        private readonly IDemandMediaService _demandMediaService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDemandProcessService _demandProcessService;
        private readonly ICompanyService _companyService;
        private readonly IDepartmentService _departmentService;
        private readonly IPersonnelService _personnelService;
        private readonly ICompanyLocationService _companyLocationService;
        private readonly IRequestInfoService _requestInfoService;
        private readonly ICurrencyTypeService _currencyTypeService;
        private readonly IDemandOfferService _demandOfferService;
        private readonly IProviderService _providerService;
        private readonly IOfferRequestService _offerRequestService;
        private readonly IPersonnelRoleService _personnelRoleService;
        private readonly IRoleService _roleService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IOfferMediaService _offerMediaService;

        public DemandsController(ILogger<HomeController> logger, IDemandService demandService, IDemandMediaService demandMediaService, IWebHostEnvironment webHostEnvironment, IDemandProcessService demandProcessService, ICompanyService companyService, IDepartmentService departmentService, IPersonnelService personnelService, ICompanyLocationService companyLocationService, IRequestInfoService requestInfoService, ICurrencyTypeService currencyTypeService, IDemandOfferService demandOfferService, IProviderService providerService, IOfferRequestService offerRequestService, IPersonnelRoleService personnelRoleService, IRoleService roleService, IProductCategoryService productCategoryService, IOfferMediaService offerMediaService)
        {
            _logger = logger;
            _demandService = demandService;
            _demandMediaService = demandMediaService;
            _webHostEnvironment = webHostEnvironment;
            _demandProcessService = demandProcessService;
            _companyService = companyService;
            _departmentService = departmentService;
            _companyLocationService = companyLocationService;
            _personnelService = personnelService;
            _requestInfoService = requestInfoService;
            _currencyTypeService = currencyTypeService;
            _demandOfferService = demandOfferService;
            _providerService = providerService;
            _offerRequestService = offerRequestService;
            _personnelRoleService = personnelRoleService;
            _roleService = roleService;
            _productCategoryService = productCategoryService;
            _offerMediaService = offerMediaService;
        }

        public IActionResult Detail(long id)
        {
            #region UserIdentity
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.Claims;
            long userId = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
            #endregion
            List<DemandOfferViewModel> demandOfferViewModels = new List<DemandOfferViewModel>();
            List<RequestInfoViewModel> requestInfoViewModels = new List<RequestInfoViewModel>();
            DemandEntity demand = _demandService.GetById(id).Data;
            List<DemandMediaEntity> demandMediaEntities = _demandMediaService.GetByDemandId(id).ToList();
            CompanyLocation companyLocation = _companyLocationService.GetById(demand.CompanyLocationId).Data;
            Company company = _companyService.GetById(companyLocation.CompanyId).Data;
            PersonnelEntity personnel = _personnelService.GetById(demand.CreatedAt).Data;
            DepartmentEntity department = _departmentService.GetById(demand.DepartmentId).Data;
            List<RequestInfoEntity> requestInfos = _requestInfoService.GetList(x => x.DemandId == id).Data.ToList();
            List<DemandOfferEntity> demandOffers = _demandOfferService.GetList(x => x.DemandId == id).Data.ToList();
            List<long> supplierIds = new List<long>();
            supplierIds = demandOffers.Select(x => x.SupplierId.Value).ToList();
            List<ProviderEntity> providerEntities = _providerService.GetList(x => supplierIds.Contains(x.Id)).Data.ToList();
            List<OfferRequestEntity> offerRequests = _offerRequestService.GetList(x => x.RequestInfoId == requestInfos[0].Id).Data.ToList();
            ViewBag.OfferRequest = offerRequests;
            DemandProcessEntity demandProcess = _demandProcessService.GetList(x => x.ManagerId == userId && x.Status == 0).Data.FirstOrDefault();
            DemandViewModel demandViewModel = new DemandViewModel
            {
                CompanyId = company.Id,
                DemandId = id,
                DemandDate = demand.CreatedDate,
                DemanderName = personnel.FirstName + " " + personnel.LastName,
                DepartmentId = demand.DepartmentId,
                Description = demand.Description,
                CreatedDate = demand.CreatedDate,
                IsDeleted = demand.IsDeleted,
                RequirementDate = demand.RequirementDate,
                CompanyLocationId = demand.CompanyLocationId,
                CreatedAt = demand.CreatedAt,
                LocationName = companyLocation.Name,
                Status = demand.Status,
                UpdatedAt = demand.UpdatedAt,
                UpdatedDate = demand.UpdatedDate,
                CompanyName = company.Name,
                DepartmentName = department.Name,
                isApprovedActive = demandProcess.IsNotNull() && demandProcess.ManagerId == userId ? true : false,
            };
            foreach (var requestInfo in requestInfos)
            {
                requestInfoViewModels.Add(new RequestInfoViewModel
                {
                    Metarial = requestInfo.ProductName,
                    Quantity = requestInfo.Quantity,
                    Unit = requestInfo.Unit
                });
            }
            demandViewModel.requestInfoViewModels = requestInfoViewModels;

            foreach (var demandOffer in demandOffers)
            {
                demandOfferViewModels.Add(new DemandOfferViewModel
                {
                    DemandId = demandOffer.DemandId,
                    CurrencyTypeId = demandOffer.CurrencyTypeId,
                    TotalPrice = demandOffer.TotalPrice,
                    Status = demandOffer.Status,
                });
            }
            demandViewModel.DemandOffers = demandOfferViewModels;
            if (demandMediaEntities.IsNotNullOrEmpty())
            {
                demandViewModel.File1Path = System.IO.File.ReadAllBytes(_webHostEnvironment.WebRootPath + demandMediaEntities[0].Path);
                demandViewModel.File1Name = demandMediaEntities[0].FileName;

                if (demandMediaEntities.Count > 1)
                {
                    demandViewModel.File2Path = System.IO.File.ReadAllBytes(_webHostEnvironment.WebRootPath + demandMediaEntities[1].Path);
                    demandViewModel.File2Name = demandMediaEntities[1].FileName;

                }
                if (demandMediaEntities.Count > 2)
                {
                    demandViewModel.File3Path = System.IO.File.ReadAllBytes(_webHostEnvironment.WebRootPath + demandMediaEntities[2].Path);
                    demandViewModel.File3Name = demandMediaEntities[2].FileName;

                }
            }
            demandViewModel.DemandOffers = new List<DemandOfferViewModel>();
            foreach (var demandOfferEntity in demandOffers.OrderBy(x => x.Id).ToList())
            {
                DemandOfferViewModel demandOfferViewModel = new DemandOfferViewModel();
                demandOfferViewModel.DemandOfferId = demandOfferEntity.Id;
                demandOfferViewModel.Status = demandOfferEntity.Status;
                demandOfferViewModel.TotalPrice = demandOfferEntity.TotalPrice;
                demandOfferViewModel.ExchangeRate = demandOfferEntity.ExchangeRate;
                demandOfferViewModel.CurrencyTypeId = demandOfferEntity.CurrencyTypeId;
                if (demandOfferEntity.SupplierId.HasValue)
                    demandOfferViewModel.SupplierId = demandOfferEntity.SupplierId.Value;
                if (!string.IsNullOrWhiteSpace(demandOfferEntity.SupplierName))
                    demandOfferViewModel.SupplierName = demandOfferEntity.SupplierName;
                if (!string.IsNullOrWhiteSpace(demandOfferEntity.SupplierPhone))
                    demandOfferViewModel.SupplierPhone = demandOfferEntity.SupplierPhone;


                ProviderEntity providerEntity = new ProviderEntity();
                if (providerEntities != null && providerEntities.Any())
                    providerEntity = providerEntities.Find(x => x.Id == demandOfferEntity.SupplierId);

                if (providerEntity != null)
                {
                    demandOfferViewModel.CompanyName = providerEntity.Name;
                    demandOfferViewModel.CompanyPhone = providerEntity.PhoneNumber;
                    demandOfferViewModel.CompanyAddress = providerEntity.Address;
                }

                List<OfferRequestViewModel> offerRequestViewModels = new List<OfferRequestViewModel>();

                foreach (var requestInfo in requestInfos)
                {
                    OfferRequestViewModel offerRequestViewModel = new OfferRequestViewModel
                    {
                        RequestInfoId = requestInfo.Id,
                        ProductCategoryId = requestInfo.ProductCategoryId,
                        DemandId = requestInfo.DemandId,
                        DemandOfferId = demandOfferEntity.Id,
                        NebimCategoryId = requestInfo.NebimCategoryId,
                        NebimSubCategoryId = requestInfo.NebimSubCategoryId,
                        ProductName = requestInfo.ProductName,
                        ProductCode = requestInfo.ProductCode,
                        Quantity = requestInfo.Quantity,
                        Unit = requestInfo.Unit
                    };

                    OfferRequestEntity? offerRequestEntity = _offerRequestService.GetFirstOrDefault(x => x.RequestInfoId == requestInfo.Id && x.DemandOfferId == demandOfferEntity.Id).Data;
                    if (offerRequestEntity != null)
                    {
                        offerRequestViewModel.OfferRequestId = offerRequestEntity.Id;
                        offerRequestViewModel.Price = offerRequestEntity.UnitPrice ?? 0;
                        offerRequestViewModel.TotalPrice = offerRequestEntity.TotalPrice ?? 0;

                        offerRequestViewModels.Add(offerRequestViewModel);
                    }

                }
                demandOfferViewModel.RequestInfoViewModels = offerRequestViewModels;
                demandViewModel.DemandOffers.Add(demandOfferViewModel);
            }


            List<Company> companies = _companyService.GetList().Data.ToList();
            ViewBag.Companies = companies;
            List<DepartmentEntity> departments = _departmentService.GetAll().Data.ToList();
            ViewBag.Department = departments;

            return View(demandViewModel);
        }

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(long id)
        {

            NebimConnection nebimConnection = new NebimConnection();
            nebimConnection.GetNebimCategoryModels();
            OfferRequestEntity? offerRequestEntity = null;
            DemandEntity demand = _demandService.GetById(id).Data;
            List<DemandMediaEntity> demandMediaEntities = _demandMediaService.GetByDemandId(id).ToList();
            CompanyLocation companyLocation = _companyLocationService.GetById(demand.CompanyLocationId).Data;
            Company company = _companyService.GetById(companyLocation.CompanyId).Data;
            PersonnelEntity personnel = _personnelService.GetById(demand.CreatedAt).Data;
            DepartmentEntity department = _departmentService.GetById(demand.DepartmentId).Data;
            List<RequestInfoEntity> requestInfos = _requestInfoService.GetList(x => x.DemandId == id).Data.ToList();
            List<DemandOfferEntity> demandOfferEntities = _demandOfferService.GetList(x => x.DemandId == id).Data.ToList();
            if (demandOfferEntities.Count > 0)
            {
                offerRequestEntity = _offerRequestService.GetFirstOrDefault(x => x.DemandOfferId == demandOfferEntities[0].Id).Data;
            }
            List<long> supplierIds = new List<long>();
            supplierIds = demandOfferEntities.Select(x => x.SupplierId.Value).ToList();
            List<ProviderEntity> providerEntities = _providerService.GetList(x => supplierIds.Contains(x.Id)).Data.ToList();
            DemandProcessEntity demandProcess = _demandProcessService.GetList(x => x.DemandId == demand.Id && x.Status == 0).Data.FirstOrDefault();

            DemandViewModel demandViewModel = new DemandViewModel
            {
                CompanyId = company.Id,
                DemandId = id,
                DemandDate = demand.CreatedDate,
                DemanderName = personnel.FirstName + " " + personnel.LastName,
                DepartmentId = demand.DepartmentId,
                Description = demand.Description,
                CreatedDate = demand.CreatedDate,
                IsDeleted = demand.IsDeleted,
                RequirementDate = demand.RequirementDate,
                CompanyLocationId = demand.CompanyLocationId,
                CreatedAt = demand.CreatedAt,
                LocationName = companyLocation.Name,
                Status = demand.Status,
                UpdatedAt = demand.UpdatedAt,
                UpdatedDate = demand.UpdatedDate,
                CompanyName = company.Name,
                DepartmentName = department.Name,
                isOppenOffer = demandProcess.ManagerId == 10 ? true : false
            };
            if (requestInfos.IsNotNullOrEmpty())
            {
                demandViewModel.Material1 = requestInfos[0].ProductName;
                demandViewModel.Quantity1 = requestInfos[0].Quantity;
                demandViewModel.Unit1 = requestInfos[0].Unit;
                if (requestInfos.Count > 1)
                {
                    demandViewModel.Material2 = requestInfos[1].ProductName;
                    demandViewModel.Quantity2 = requestInfos[1].Quantity;
                    demandViewModel.Unit2 = requestInfos[1].Unit;
                }
                if (requestInfos.Count > 2)
                {
                    demandViewModel.Material3 = requestInfos[2].ProductName;
                    demandViewModel.Quantity3 = requestInfos[2].Quantity;
                    demandViewModel.Unit3 = requestInfos[2].Unit;
                }
            }
            demandViewModel.DemandOffers = new List<DemandOfferViewModel>();
            foreach (var demandOfferEntity in demandOfferEntities.OrderBy(x => x.Id).ToList())
            {
                DemandOfferViewModel demandOfferViewModel = new DemandOfferViewModel();
                demandOfferViewModel.DemandOfferId = demandOfferEntity.Id;
                demandOfferViewModel.TotalPrice = demandOfferEntity.TotalPrice;
                if (demandOfferEntity.SupplierId.HasValue)
                    demandOfferViewModel.SupplierId = demandOfferEntity.SupplierId.Value;
                if (!string.IsNullOrWhiteSpace(demandOfferEntity.SupplierName))
                    demandOfferViewModel.SupplierName = demandOfferEntity.SupplierName;
                if (!string.IsNullOrWhiteSpace(demandOfferEntity.SupplierPhone))
                    demandOfferViewModel.SupplierPhone = demandOfferEntity.SupplierPhone;
                if (!string.IsNullOrWhiteSpace(demandOfferEntity.SupplierAdress))
                    demandOfferViewModel.SupplierAdress = demandOfferEntity.SupplierAdress;
                if (demandOfferEntity.DeadlineDate.IsNotNull())
                {
                    demandOfferViewModel.DeadlineDate = demandOfferEntity.DeadlineDate;

                }
                if (demandOfferEntity.MaturityDate.IsNotNull())
                {
                    demandOfferViewModel.MaturityDate = demandOfferEntity.MaturityDate;

                }

                ProviderEntity providerEntity = new ProviderEntity();
                if (providerEntities != null && providerEntities.Any())
                    providerEntity = providerEntities.Find(x => x.Id == demandOfferEntity.SupplierId);

                if (providerEntity != null)
                {
                    demandOfferViewModel.CompanyName = providerEntity.Name;
                    demandOfferViewModel.CompanyPhone = providerEntity.PhoneNumber;
                    demandOfferViewModel.CompanyAddress = providerEntity.Address;
                }
                demandViewModel.DemandOffers.Add(demandOfferViewModel);
            }
            List<CurrencyTypeEntity> currencyTypes = _currencyTypeService.GetAll().Data.ToList();
            ViewBag.CurrencyTypes = currencyTypes;
            List<CompanyLocation> locations = _companyLocationService.GetAll().Data.ToList();
            ViewBag.Locations = locations;
            List<Company> companies = _companyService.GetList().Data.ToList();
            ViewBag.Companies = companies;
            List<DepartmentEntity> departments = _departmentService.GetAll().Data.ToList();
            ViewBag.Departments = departments;
            List<ProviderEntity> providers = _providerService.GetAll().Data.ToList();
            ViewBag.Providers = providers;
            ViewBag.OfferRequest = offerRequestEntity;
            return View(demandViewModel);

        }
        private string ConvertFileToBase64(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }

        [HttpPost("AddDemand")]
        public IActionResult AddDemand([FromForm] DemandViewModel demandViewModel)
        {
            #region UserIdentity
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.Claims;
            #endregion

            if (demandViewModel == null)
            {
                return BadRequest("Invalid demand data");
            }
            var demandEntity = new DemandEntity
            {
                CompanyLocationId = (long)demandViewModel.CompanyLocationId,
                LocationUnitId = (long)demandViewModel.LocationUnitId,
                DemandTitle = demandViewModel.DemandTitle,
                DepartmentId = (long)demandViewModel.DepartmentId,
                Status = 0,
                Description = demandViewModel.Description,
                RequirementDate = (DateTime)demandViewModel.RequirementDate,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                UpdatedDate = null,
                CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value),
                UpdatedAt = null,
            };
            var addedDemand = _demandService.AddDemand(demandEntity);

            for (int i = 0; i < demandViewModel.ProductName.Count; i++)
            {
                var type = demandViewModel.Type[i] == "Lütfen seçiniz" ? null : demandViewModel.Type[i];
                var category = demandViewModel.Category[i] == "Lütfen seçiniz" ? null : demandViewModel.Category[i];
                var subcategory = demandViewModel.Subcategory[i] == null ? null : demandViewModel.Subcategory[i];
                var unit = demandViewModel.Unit[i];
                var quantity = demandViewModel.Quantity[i];
                var productname = demandViewModel.ProductName[i];
                var productcode = demandViewModel.ProductCode[i];
                var requestInfo = new RequestInfoEntity
                {
                    DemandId = addedDemand.Id,
                    NebimCategoryId = Convert.ToInt32(category).IsNotNull() && Convert.ToInt32(category) !=0 ? Convert.ToInt32(category): null,
                    NebimSubCategoryId = Convert.ToInt32(subcategory).IsNotNull() && Convert.ToInt32(subcategory)!=0 ? Convert.ToInt32(subcategory) :null ,
                    Quantity = Convert.ToInt32(quantity).IsNotNull() && Convert.ToInt32(quantity) != 0 ? Convert.ToInt32(quantity) : null,
                    ProductName = productname,
                    ProductCode = productcode,
                    Unit = unit,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = null,
                    ProductCategoryId = Convert.ToInt32(type).IsNotNull() && Convert.ToInt32(type)!= 0 ? Convert.ToInt32(type) : null,
                    CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value),
                    UpdatedAt = null,
                    IsFirst = true,
                };

                var requestInfoAdd = _requestInfoService.Add(requestInfo);
            }

            if (demandViewModel.Files != null && demandViewModel.Files.Count > 0)
            {

                foreach (var file in demandViewModel.Files)
                {
                    string fileName = addedDemand.Id + "_" + file.FileName;

                    DemandMediaEntity demandMediaEntity = SaveFileAndCreateEntity(file, addedDemand.Id);
                    demandMediaEntity.DemandId = addedDemand.Id;
                    demandMediaEntity.Path = demandMediaEntity.Path;
                    demandMediaEntity.FileName = file.FileName;
                    demandMediaEntity.IsDeleted = false;
                    demandMediaEntity.CreatedDate = DateTime.Now;
                    demandMediaEntity.UpdatedDate = null;
                    demandMediaEntity.CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
                    demandMediaEntity.UpdatedAt = null;
                    _demandMediaService.AddDemandMedia(demandMediaEntity);

                }
            }


            PersonnelEntity personnelEntity = _personnelService.GetById(long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value)).Data;

            if (personnelEntity.ParentId.IsNotNull())
            {
                PersonnelEntity parentPersonnel = _personnelService.GetById((long)personnelEntity.ParentId).Data;

                int i = 0;
                while (parentPersonnel != null)
                {
                    i++;
                    DemandProcessEntity demandProcessEntity = new DemandProcessEntity
                    {
                        DemandId = addedDemand.Id,
                        ManagerId = parentPersonnel.Id,
                        IsDeleted = false,
                        HierarchyOrder = i,
                        Desciription = string.Empty,
                        Status = 0,
                        CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value),
                        CreatedDate = DateTime.Now,
                        UpdatedAt = null,
                        UpdatedDate = null,
                    };
                    _demandProcessService.AddDemandProcess(demandProcessEntity);
                    if (i == 1)
                    {
                        if (!string.IsNullOrWhiteSpace(parentPersonnel.Email))
                        {
                            string demandLink = "http://172.30.44.13:5734/api/Demands?id=" + demandProcessEntity.DemandId;
                            var emailBody = $"Merhabalar Sayın {parentPersonnel.FirstName} {parentPersonnel.LastName},<br/><br/>" +
                                            $"{personnelEntity.FirstName} {personnelEntity.LastName} tarafından, {demandEntity.DemandTitle} başlıklı, {demandEntity.Id} numaralı satın alma talebi açılmıştır. Aşağıdaki linkten talebi kontrol ederek onay vermenizi rica ederiz.<br/><br/>" +
                                            $"Talep URL : <a href='{demandLink}'> TALEP GÖRÜNTÜLE </a> <br/><br/>" +
                                            "Saygılarımızla.";
                            EmailHelper.SendEmail(new List<string> { parentPersonnel.Email }, "Onayınızı Bekleyen Satın Alma Talebi", emailBody);
                        }
                    }

                    //if (parentPersonnel.ParentId != null)
                    //    parentPersonnel = _personnelService.GetById((long)parentPersonnel.ParentId).Data;
                    //else
                    //    break;
                    break;
                }
                RoleEntity roleEntity = _roleService.GetById((int)PersonnelRoleEnum.SatınAlmaManager).Data;
                List<PersonnelRoleEntity> personnelRole = _personnelRoleService.GetList(x => x.RoleId == roleEntity.Id).Data.ToList();
                PersonnelEntity personnelForSales = _personnelService.GetById(personnelRole[0].PersonnelId).Data;

                i++;
                DemandProcessEntity demandProcessForOffer = new DemandProcessEntity
                {
                    DemandId = addedDemand.Id,
                    ManagerId = personnelForSales.Id,
                    IsDeleted = false,
                    HierarchyOrder = i,
                    Desciription = string.Empty,
                    Status = 0,
                    CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value),
                    CreatedDate = DateTime.Now,
                    UpdatedAt = null,
                    UpdatedDate = null,
                };
                _demandProcessService.AddDemandProcess(demandProcessForOffer);


                PersonnelEntity FirstApprovedPersonnel = _personnelService.GetById((long)personnelEntity.ParentId).Data;

                i++;
                DemandProcessEntity demandProcessFirstApprovedManager = new DemandProcessEntity
                {
                    DemandId = addedDemand.Id,
                    ManagerId = FirstApprovedPersonnel.Id,
                    IsDeleted = false,
                    HierarchyOrder = i,
                    Desciription = string.Empty,
                    Status = 0,
                    CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value),
                    CreatedDate = DateTime.Now,
                    UpdatedAt = null,
                    UpdatedDate = null,
                };
                _demandProcessService.AddDemandProcess(demandProcessFirstApprovedManager);

                i++;

                DemandProcessEntity demandProcessLastManager = new DemandProcessEntity
                {
                    DemandId = addedDemand.Id,
                    ManagerId = (int)FirstApprovedPersonnel.ParentId,
                    IsDeleted = false,
                    HierarchyOrder = i,
                    Desciription = string.Empty,
                    Status = 0,
                    CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value),
                    CreatedDate = DateTime.Now,
                    UpdatedAt = null,
                    UpdatedDate = null,
                };
                _demandProcessService.AddDemandProcess(demandProcessLastManager);

            }
            else
            {
                PersonnelEntity personnel = _personnelService.GetById(10).Data;

                string demandLink = "http://172.30.44.13:5734/api/Demands/Edit/" + addedDemand.Id;
                var emailBody = $"Merhabalar Sayın {personnel.FirstName} {personnel.LastName},<br/><br/>" +
                     $"{personnelEntity.FirstName} {personnelEntity.LastName} tarafından, {demandEntity.DemandTitle} başlıklı, {demandEntity.Id} numaralı satın alma talebi açılmıştır. Aşağıdaki linkten talebi kontrol etmenizi rica ederiz.<br/><br/>" +
                     $"Talep URL : <a href='{demandLink}'>  TALEP GÖRÜNTÜLE  </a> <br/><br/>" +
                     "Saygılarımızla.";
                EmailHelper.SendEmail(new List<string> { personnel.Email }, "Onayınızı Bekleyen Satın Alma Talebi", emailBody);
            }
            return Ok(addedDemand);
        }

        [HttpPut("ChangeStatus")]
        public IActionResult ChangeStatus([FromBody] DemandStatusChangeViewModel demandStatusChangeViewModel)
        {
            if (demandStatusChangeViewModel.Status != 1 && demandStatusChangeViewModel.Status != 2)
            {
                return BadRequest("Talep Sadece İptal ve Onay Durumuna Alınabilir.");
            }
            if (demandStatusChangeViewModel.Status == 1 && string.IsNullOrWhiteSpace(demandStatusChangeViewModel.Description))
            {
                return BadRequest("Talep İptal Edilirken Açıklama Girmek Zorunludur.");
            }

            #region UserIdentity
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.Claims;
            #endregion

            List<DemandProcessEntity> demandProcessEntities = _demandProcessService.GetList(x => x.DemandId == demandStatusChangeViewModel.DemandId).Data.ToList();

            DemandProcessEntity? demandProcessEntity = demandProcessEntities.FirstOrDefault(x => x.ManagerId == long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value) && x.Status == 0);
            if (demandProcessEntity == null)
            {
                return BadRequest("Talebe Ait Durum Değiştirme Yetkiniz Bulunmamaktadır.");
            }

            if (demandProcessEntity.Status != 0)
            {
                return BadRequest("Talep Durumu Değiştirmeye Uygun Değildir.");
            }

            demandProcessEntity.Status = (int)demandStatusChangeViewModel.Status;
            demandProcessEntity.Desciription = demandStatusChangeViewModel.Description ?? string.Empty;
            demandProcessEntity.UpdatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
            demandProcessEntity.UpdatedDate = DateTime.Now;

            _demandProcessService.UpdateDemandProcess(demandProcessEntity);

            PersonnelRoleEntity personnelRole = _personnelRoleService.GetList(x => x.PersonnelId == demandProcessEntity.UpdatedAt).Data.FirstOrDefault();


            if (demandProcessEntity.Status == 2)
            {
                DemandProcessEntity? nextDemandProcessEntity = demandProcessEntities.FirstOrDefault(x => x.HierarchyOrder == demandProcessEntity.HierarchyOrder + 1);
                if (nextDemandProcessEntity != null)
                {
                    PersonnelEntity personnel = _personnelService.GetById(nextDemandProcessEntity.ManagerId).Data;
                    PersonnelEntity demandOpenPerson = _personnelService.GetById(demandProcessEntity.CreatedAt).Data;
                    DemandEntity demand = _demandService.GetById(demandProcessEntity.DemandId).Data;

                    if (nextDemandProcessEntity.ManagerId == 10)
                    {

                        string demandLink = "http://172.30.44.13:5734/api/Demands/Edit/" + demandProcessEntity.DemandId;
                        var emailBody = $"Merhabalar Sayın " + personnel.FirstName + " " + personnel.LastName + ",<br/><br/>" +
                                    demandOpenPerson.FirstName + " " + demandOpenPerson.LastName + " tarafından," + demand.DemandTitle + " başlıklı," + demand.Id + " numaralı satın alma talebi açılmış ve onaylanmıştır. Lütfen teklif ve diğer detay bilgileri doldurmanızı rica ederiz.<br/><br/>" +
                                     $"Talep URL : <a href='{demandLink}'>  TALEP GÖRÜNTÜLE  </a> <br/><br/>" +
                     "Saygılarımızla.";
                        EmailHelper.SendEmail(new List<string> { personnel.Email }, "Teklif Girişi Bekleyen Satın Alma Talebi", emailBody);
                    }
                    else if (personnelRole != null && personnelRole.RoleId != 8)
                    {
                        string demandLink = "http://172.30.44.13:5734/api/Demands/DemandOfferDetail?DemandId=" + demandProcessEntity.DemandId;
                        var emailBody = $"Merhabalar Sayın " + personnel.FirstName + " " + personnel.LastName + ",<br/><br/>" +
                                    demandOpenPerson.FirstName + " " + demandOpenPerson.LastName + " tarafından," + demand.DemandTitle + " başlıklı," + demand.Id + " numaralı satın alma talebine girilen teklifler birim yöneticisi tarafından değelendirilmiştir. Aşağıdaki linkten talep içerisindeki teklifleri değerlendirerek onay vermenizi rica ederiz.<br/><br/>" +
                                     $"Talep URL : <a href='{demandLink}'>  TALEP GÖRÜNTÜLE  </a> <br/><br/>" +
                     "Saygılarımızla.";
                        EmailHelper.SendEmail(new List<string> { personnel.Email }, "Onayınızı Bekleyen Satın Alma Talebi", emailBody);

                        if (demandStatusChangeViewModel.DemandOfferId != null)
                        {
                            List<DemandOfferEntity> demandOfferEntities = _demandOfferService.GetList(x => x.DemandId == demandStatusChangeViewModel.DemandId).Data.ToList();

                            foreach (var demandOffer in demandOfferEntities)
                            {
                                demandOffer.UnitManager = demandOffer.Id == demandStatusChangeViewModel.DemandOfferId ? 2 : 1;

                                _demandOfferService.Update(demandOffer);
                            }
                        }
                    }
                    else
                    {
                        string demandLink = "http://172.30.44.13:5734/api/Demands/DemandOfferDetail?DemandId=" + demandProcessEntity.DemandId;
                        var emailBody = $"Merhabalar Sayın " + personnel.FirstName + " " + personnel.LastName + ",<br/><br/>" +
                                    demandOpenPerson.FirstName + " " + demandOpenPerson.LastName + " tarafından," + demand.DemandTitle + " başlıklı," + demand.Id + " numaralı satın alma talebi açılmıştır. Aşağıdaki linkten talebi kontrol ederek onay vermenizi rica ederiz.<br/><br/>" +
                                     $"Talep URL : <a href='{demandLink}'>  TALEP GÖRÜNTÜLE  </a> <br/><br/>" +
                     "Saygılarımızla.";
                        EmailHelper.SendEmail(new List<string> { personnel.Email }, "Onayınızı Bekleyen Satın Alma Talebi", emailBody);
                    }
                }

                else
                {
                    DemandEntity demandEntity = _demandService.GetById((long)demandStatusChangeViewModel.DemandId).Data;
                    demandEntity.Status = 2;
                    demandEntity.UpdatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
                    demandEntity.UpdatedDate = DateTime.Now;

                    if (demandStatusChangeViewModel.DemandOfferId != null)
                    {
                        List<DemandOfferEntity> demandOfferEntities = _demandOfferService.GetList(x => x.DemandId == demandStatusChangeViewModel.DemandId).Data.ToList();

                        foreach (var demandOffer in demandOfferEntities)
                        {
                            demandOffer.Status = demandOffer.Id == demandStatusChangeViewModel.DemandOfferId ? 2 : 1;
                            demandEntity.UpdatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
                            demandEntity.UpdatedDate = DateTime.Now;

                            _demandOfferService.Update(demandOffer);
                        }
                    }

                    PersonnelEntity personnel = _personnelService.GetById(10).Data;
                    PersonnelEntity demandOpenPerson = _personnelService.GetById(demandProcessEntity.CreatedAt).Data;
                    string demandLink = "";
                    var emailBody = "";
                    demandLink = "http://172.30.44.13:5734/api/Demands/Edit/" + demandProcessEntity.DemandId;
                    emailBody = $"Merhabalar Sayın " + personnel.FirstName + " " + personnel.LastName + ",<br/><br/>" +
                               demandOpenPerson.FirstName + " " + demandOpenPerson.LastName + " tarafından," + demandEntity.DemandTitle + " başlıklı," + demandEntity.Id + " numaralı satın alma talebi onaylanmıştır.Bilginize sunarız.<br/><br/>" + $"Talep URL : <a href='{demandLink}'>  TALEP GÖRÜNTÜLE  </a> <br/><br/>" +
                    "Saygılarımızla.";
                    EmailHelper.SendEmail(new List<string> { personnel.Email }, "Onaylanan Satın Alma Talebi", emailBody);

                    /*Finans Mail*/

                    demandLink = "http://172.30.44.13:5734/api/Demands/Edit/" + demandProcessEntity.DemandId;
                    emailBody = $"Merhabalar Sayın  Okan KÜÇÜK   ,<br/><br/>" /*+ personnel.FirstName + " " + personnel.LastName + ",<br/><br/>" +*/+
                               demandOpenPerson.FirstName + " " + demandOpenPerson.LastName + " tarafından," + demandEntity.DemandTitle + " başlıklı," + demandEntity.Id + " numaralı satın alma talebi onaylanmıştır.Bilginize sunarız.<br/><br/>" + $"Talep URL : <a href='{demandLink}'>  TALEP GÖRÜNTÜLE  </a> <br/><br/>" +
                    "Saygılarımızla.";
                    EmailHelper.SendEmail(new List<string> { "okan.kucuk@demmuseums.com" }, "Onaylanan Satın Alma Talebi", emailBody);
                    _demandService.Update(demandEntity);
                }
            }
            else if (demandProcessEntity.Status == 1)//Cancelled
            {
                DemandEntity demandEntity = _demandService.GetById((long)demandStatusChangeViewModel.DemandId).Data;
                demandEntity.Status = 1;
                demandEntity.UpdatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
                demandEntity.UpdatedDate = DateTime.Now;

                _demandService.Update(demandEntity);

                List<DemandProcessEntity> previousDemandProcessList = demandProcessEntities.Where(x => x.HierarchyOrder < demandProcessEntity.HierarchyOrder).ToList();
                if (previousDemandProcessList != null && previousDemandProcessList.Count > 0)
                {
                    foreach (var demandProcess in previousDemandProcessList)
                    {
                        if (demandProcess.ManagerId.IsNotNull())
                        {
                            PersonnelEntity personnel = _personnelService.GetById(demandProcess.ManagerId).Data;

                            var emailBody = $"Merhabalar Sayın " + personnel.FirstName + " " + personnel.LastName + ",<br/><br/>" +
                                        demandEntity.Id + " numaralı satın alma talebi iptal edilmiştir. Bilginize sunarız.<br/><br/>" +
                                        "Saygılarımızla.";
                            EmailHelper.SendEmail(new List<string> { personnel.Email }, "İptal Edilen Satın Alma Talebi", emailBody);
                        }
                    }
                }
                PersonnelEntity personnelEntity = _personnelService.GetById(demandProcessEntities[0].CreatedAt).Data;
                if (personnelEntity.IsNotNull())
                {
                    var emailBody = $"Merhabalar Sayın " + personnelEntity.FirstName + " " + personnelEntity.LastName + ",<br/><br/>" +
                                demandEntity.Id + " numaralı satın alma talebiniz iptal edilmiştir. Bilginize sunarız.<br/><br/>" +
                                "Saygılarımızla.";
                    EmailHelper.SendEmail(new List<string> { personnelEntity.Email }, "İptal Edilen Satın Alma Talebi", emailBody);
                }


            }

            return Ok(demandProcessEntity);
        }
        private byte[] GetFile(IFormFile file)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();
            return fileBytes;
        }
        private DemandMediaEntity SaveFileAndCreateEntity(IFormFile file, long demandId)
        {
            if (file != null && file.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                //string partToRemove = @"Demand.Presentation";
                string newPath = uploadsFolder;
                newPath = newPath.Replace(@"\\", @"\");
                string uniqueFileName = demandId + "_" + file.FileName;
                string filePath = Path.Combine(newPath, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return new DemandMediaEntity
                {
                    DemandId = demandId,
                    Path = "\\uploads\\" + uniqueFileName
                };
            }

            return null;
        }

        private OfferMediaEntity SaveFileAndCreateOfferMedia(string base64FileContent, string fileName, long offerId)
        {
            if (!string.IsNullOrEmpty(base64FileContent) && !string.IsNullOrEmpty(fileName))
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = $"{offerId}_{fileName}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                byte[] fileBytes = Convert.FromBase64String(base64FileContent);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    fileStream.Write(fileBytes, 0, fileBytes.Length);
                }

                return new OfferMediaEntity
                {
                    OfferId = offerId,
                    Path = $"\\uploads\\{uniqueFileName}",
                    FileName = fileName
                };
            }

            return null;
        }

        private string RemovePathPart(string originalPath, string partToRemove)
        {
            int index = originalPath.IndexOf(partToRemove, StringComparison.OrdinalIgnoreCase);
            if (index == -1)
            {
                return originalPath;
            }

            string beforePart = originalPath.Substring(0, index);

            string afterPart = originalPath.Substring(index + partToRemove.Length);
            string newPath = Path.Combine(beforePart, afterPart).TrimStart(Path.DirectorySeparatorChar);

            return newPath;
        }
        private void AddDemandProcess()
        {

        }
        [HttpPost("UpdateDemand")]
        public IActionResult UpdateDemand([FromBody] UpdateDemandViewModel updateDemandViewModel)
        {
            ProviderEntity providerEntity = new ProviderEntity();
            ProviderEntity providerEntity2 = new ProviderEntity();
            ProviderEntity providerEntity3 = new ProviderEntity();

            #region UserIdentity
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.Claims;
            long userId = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
            #endregion

            #region AddProvider
            if (!updateDemandViewModel.IsProvider1Registered)
            {
                providerEntity.Name = updateDemandViewModel.OfferCompanyName;
                providerEntity.PhoneNumber = updateDemandViewModel.OfferCompanyPhone;
                providerEntity.Address = updateDemandViewModel.OfferCompanyAddress;
                providerEntity.CreatedDate = DateTime.Now;
                providerEntity.CreatedAt = userId;
                providerEntity.UpdatedDate = null;
                providerEntity.UpdatedAt = null;
                _providerService.Add(providerEntity);
            }
            #endregion

            #region RequestInfo
            RequestInfoEntity requestInfo = _requestInfoService.GetByDemandId((long)updateDemandViewModel.DemandId).Data;
            if (updateDemandViewModel.OfferCompanyName.IsNotNull())
            {
                DemandOfferEntity demandOfferEntity = new DemandOfferEntity();
                demandOfferEntity.CreatedAt = userId;
                demandOfferEntity.CreatedDate = DateTime.Now;
                demandOfferEntity.CurrencyTypeId = (long)updateDemandViewModel.OfferCurrencyType;
                demandOfferEntity.DemandId = (long)updateDemandViewModel.DemandId;
                demandOfferEntity.SupplierName = updateDemandViewModel.OfferCompanyName;
                demandOfferEntity.SupplierPhone = updateDemandViewModel.OfferCompanyPhone;
                demandOfferEntity.SupplierId = updateDemandViewModel.OfferCompanyId.IsNotNull() ? updateDemandViewModel.OfferCompanyId.Value : providerEntity.Id;
                demandOfferEntity.IsDeleted = false;
                //demandOfferEntity.RequestInfoId = requestInfo.Id;
                demandOfferEntity.SupplierAdress = updateDemandViewModel.OfferCompanyAddress;
                demandOfferEntity.Status = 0;
                demandOfferEntity.TotalPrice = updateDemandViewModel.OfferTotalPrice;
                demandOfferEntity.UpdatedAt = null;
                demandOfferEntity.UpdatedDate = null;
                demandOfferEntity.ExchangeRate = updateDemandViewModel.ExchangeRate;
                demandOfferEntity.UnitManager = 0;
                demandOfferEntity.DeadlineDate = updateDemandViewModel.DeadlineDate;
                demandOfferEntity.MaturityDate = updateDemandViewModel.MaturityDate;
                demandOfferEntity.PaymentType = updateDemandViewModel.PaymentType;
                demandOfferEntity.PartialPayment = updateDemandViewModel.PartialPayment;
                demandOfferEntity.InstallmentPayment = updateDemandViewModel.InstallmentPayment;

                var demandOfferAdd = _demandOfferService.Add(demandOfferEntity);
                if (!string.IsNullOrEmpty(updateDemandViewModel.FileContent) && !string.IsNullOrEmpty(updateDemandViewModel.FileName))
                {
                    OfferMediaEntity offerMediaEntity = SaveFileAndCreateOfferMedia(updateDemandViewModel.FileContent, updateDemandViewModel.FileName, demandOfferAdd.Id);

                    if (offerMediaEntity != null)
                    {
                        offerMediaEntity.IsDeleted = false;
                        offerMediaEntity.CreatedDate = DateTime.Now;
                        offerMediaEntity.UpdatedDate = null;
                        offerMediaEntity.CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
                        offerMediaEntity.UpdatedAt = null;
                        offerMediaEntity.DemandId = updateDemandViewModel.DemandId;

                        _offerMediaService.AddOfferMedia(offerMediaEntity);
                    }
                }
                #region
                //if (demandOfferAdd.Id.IsNotNull())
                //{
                //    OfferRequestEntity offerRequestEntity = new OfferRequestEntity();
                //    offerRequestEntity.Status = 0;
                //    offerRequestEntity.DemandOfferId = demandOfferEntity.Id;
                //    offerRequestEntity.UpdatedDate = null;
                //    offerRequestEntity.CreatedDate= DateTime.Now;
                //    offerRequestEntity.UpdatedAt = null;
                //    offerRequestEntity.CreatedAt = userId;
                //    offerRequestEntity.IsDeleted = false;
                //    offerRequestEntity.RequestInfoId = requestInfo.Id;
                //    offerRequestEntity.TotalPrice = updateDemandViewModel.OfferTotalPrice;
                //    offerRequestEntity.UnitPrice = updateDemandViewModel.OfferPrice;
                //    _offerRequestService.Add(offerRequestEntity);
                //}
                #endregion
            }
            #endregion


            #region UpdateDemand
            DemandEntity demandEntity = _demandService.GetById(updateDemandViewModel.DemandId.Value).Data;
            string title = demandEntity.DemandTitle;
            demandEntity.CompanyLocationId = updateDemandViewModel.CompanyLocationId.Value;
            demandEntity.DepartmentId = updateDemandViewModel.DepartmentId.Value;
            demandEntity.Description = updateDemandViewModel.Description;
            demandEntity.DemandTitle = updateDemandViewModel.DemandTitle;
            demandEntity.UpdatedAt = userId;
            demandEntity.UpdatedDate = DateTime.Now;
            demandEntity.DemandTitle = title;
            _demandService.Update(demandEntity);
            #endregion

            return Ok(demandEntity);
        }
        [HttpGet("DemandOfferDetail")]
        public IActionResult DemandOfferDetail(long DemandId)
        {
            try
            {
                #region UserIdentity
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.Claims;
                long userId = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
                #endregion
                NebimConnection nebimConnection = new NebimConnection();
                nebimConnection.GetNebimCategoryModels();

                DemandEntity demand = _demandService.GetById(DemandId).Data;
                List<DemandMediaEntity> demandMediaEntities = _demandMediaService.GetByDemandId(DemandId).ToList();
                List<OfferMediaEntity> offerMediaEntities = _offerMediaService.GetByDemandId(DemandId).ToList();
                CompanyLocation companyLocation = _companyLocationService.GetById(demand.CompanyLocationId).Data;
                Company company = _companyService.GetById(companyLocation.CompanyId).Data;
                PersonnelEntity personnel = _personnelService.GetById(demand.CreatedAt).Data;
                DepartmentEntity department = _departmentService.GetById(demand.DepartmentId).Data;
                List<RequestInfoEntity> requestInfos = _requestInfoService.GetList(x => x.DemandId == DemandId).Data.ToList();
                List<DemandOfferEntity> demandOfferEntities = _demandOfferService.GetList(x => x.DemandId == DemandId).Data.ToList();
                List<long> supplierIds = new List<long>();
                supplierIds = demandOfferEntities.Select(x => x.SupplierId.Value).ToList();
                List<ProviderEntity> providerEntities = _providerService.GetList(x => supplierIds.Contains(x.Id)).Data.ToList();
                DemandProcessEntity demandProcess = _demandProcessService.GetList(x => x.Desciription != null && x.Desciription != "" && x.DemandId == DemandId).Data.FirstOrDefault();
                DemandProcessEntity isApprovedActiveProcess = _demandProcessService.GetList(x => x.ManagerId == userId && x.Status == 0 && x.DemandId == demand.Id).Data.FirstOrDefault();


                DemandViewModel demandViewModel = new DemandViewModel
                {
                    CompanyId = company.Id,
                    DemandId = DemandId,
                    DemandDate = demand.CreatedDate,
                    DemanderName = personnel.FirstName + " " + personnel.LastName,
                    DepartmentId = demand.DepartmentId,
                    Description = demand.Description,
                    CreatedDate = demand.CreatedDate,
                    IsDeleted = demand.IsDeleted,
                    RequirementDate = demand.RequirementDate,
                    CompanyLocationId = demand.CompanyLocationId,
                    CreatedAt = demand.CreatedAt,
                    LocationName = companyLocation.Name,
                    Status = demand.Status,
                    UpdatedAt = demand.UpdatedAt,
                    UpdatedDate = demand.UpdatedDate,
                    CompanyName = company.Name,
                    DepartmentName = department.Name,
                    ConfirmingNote = demandProcess.IsNotNull() ? demandProcess.Desciription : "",
                    isApprovedActive = isApprovedActiveProcess.IsNotNull() && isApprovedActiveProcess.ManagerId == userId ? true : false

                };
                if (requestInfos.IsNotNullOrEmpty())
                {
                    demandViewModel.Material1 = requestInfos[0].ProductName;
                    demandViewModel.Quantity1 = requestInfos[0].Quantity;
                    demandViewModel.Unit1 = requestInfos[0].Unit;
                    if (requestInfos.Count > 1)
                    {
                        demandViewModel.Material2 = requestInfos[1].ProductName;
                        demandViewModel.Quantity2 = requestInfos[1].Quantity;
                        demandViewModel.Unit2 = requestInfos[1].Unit;

                    }
                    if (requestInfos.Count > 2)
                    {
                        demandViewModel.Material3 = requestInfos[2].ProductName;
                        demandViewModel.Quantity3 = requestInfos[2].Quantity;
                        demandViewModel.Unit3 = requestInfos[2].Unit;

                    }
                }
                if (demandMediaEntities.IsNotNullOrEmpty())
                {
                    demandViewModel.File1Path = System.IO.File.ReadAllBytes(_webHostEnvironment.WebRootPath + demandMediaEntities[0].Path);
                    demandViewModel.File1Name = demandMediaEntities[0].FileName;

                    if (demandMediaEntities.Count > 1)
                    {
                        demandViewModel.File2Path = System.IO.File.ReadAllBytes(_webHostEnvironment.WebRootPath + demandMediaEntities[1].Path);
                        demandViewModel.File2Name = demandMediaEntities[1].FileName;

                    }
                    if (demandMediaEntities.Count > 2)
                    {
                        demandViewModel.File3Path = System.IO.File.ReadAllBytes(_webHostEnvironment.WebRootPath + demandMediaEntities[2].Path);
                        demandViewModel.File3Name = demandMediaEntities[2].FileName;
                    }
                }

                if (offerMediaEntities.IsNotNullOrEmpty())
                {
                    demandViewModel.ProformoFile1Path = System.IO.File.ReadAllBytes(_webHostEnvironment.WebRootPath + offerMediaEntities[0].Path);
                    demandViewModel.ProformoFile1Name = offerMediaEntities[0].FileName;

                    if (offerMediaEntities.Count > 1)
                    {
                        demandViewModel.ProformoFile2Path = System.IO.File.ReadAllBytes(_webHostEnvironment.WebRootPath + offerMediaEntities[1].Path);
                        demandViewModel.ProformoFile2Name = offerMediaEntities[1].FileName;

                    }
                    if (offerMediaEntities.Count > 2)
                    {
                        demandViewModel.ProformoFile3Path = System.IO.File.ReadAllBytes(_webHostEnvironment.WebRootPath + offerMediaEntities[2].Path);
                        demandViewModel.ProformoFile3Name = offerMediaEntities[2].FileName;
                    }
                }

                demandViewModel.DemandOffers = new List<DemandOfferViewModel>();
                foreach (var demandOfferEntity in demandOfferEntities.OrderBy(x => x.Id).ToList())
                {
                    DemandOfferViewModel demandOfferViewModel = new DemandOfferViewModel();
                    demandOfferViewModel.DemandOfferId = demandOfferEntity.Id;
                    demandOfferViewModel.Status = demandOfferEntity.Status;
                    demandOfferViewModel.UnitManager = (int)demandOfferEntity.UnitManager;
                    demandOfferViewModel.TotalPrice = demandOfferEntity.TotalPrice;
                    demandOfferViewModel.ExchangeRate = demandOfferEntity.ExchangeRate;
                    demandOfferViewModel.CurrencyTypeId = demandOfferEntity.CurrencyTypeId;
                    demandOfferViewModel.PaymentType = demandOfferEntity.PaymentType;
                    demandOfferViewModel.InstallmentPayment = demandOfferEntity.InstallmentPayment;
                    demandOfferViewModel.PartialPayment = demandOfferEntity.PartialPayment;
                    demandOfferViewModel.DeadlineDate = demandOfferEntity.DeadlineDate;
                    demandOfferViewModel.MaturityDate = demandOfferEntity.MaturityDate;

                    if (demandOfferEntity.SupplierId.HasValue)
                        demandOfferViewModel.SupplierId = demandOfferEntity.SupplierId.Value;
                    if (!string.IsNullOrWhiteSpace(demandOfferEntity.SupplierName))
                        demandOfferViewModel.SupplierName = demandOfferEntity.SupplierName;
                    if (!string.IsNullOrWhiteSpace(demandOfferEntity.SupplierPhone))
                        demandOfferViewModel.SupplierPhone = demandOfferEntity.SupplierPhone;


                    ProviderEntity providerEntity = new ProviderEntity();
                    if (providerEntities != null && providerEntities.Any())
                        providerEntity = providerEntities.Find(x => x.Id == demandOfferEntity.SupplierId);

                    if (providerEntity != null)
                    {
                        demandOfferViewModel.CompanyName = providerEntity.Name;
                        demandOfferViewModel.CompanyPhone = providerEntity.PhoneNumber;
                        demandOfferViewModel.CompanyAddress = providerEntity.Address;
                    }

                    List<OfferRequestViewModel> offerRequestViewModels = new List<OfferRequestViewModel>();

                    foreach (var requestInfo in requestInfos)
                    {
                        OfferRequestViewModel offerRequestViewModel = new OfferRequestViewModel
                        {
                            RequestInfoId = requestInfo.Id,
                            ProductCategoryId = requestInfo.ProductCategoryId,
                            DemandId = requestInfo.DemandId,
                            DemandOfferId = demandOfferEntity.Id,
                            NebimCategoryId = requestInfo.NebimCategoryId,
                            NebimSubCategoryId = requestInfo.NebimSubCategoryId,
                            ProductName = requestInfo.ProductName,
                            ProductCode = requestInfo.ProductCode,
                            Quantity = requestInfo.Quantity,
                            Unit = requestInfo.Unit
                        };

                        OfferRequestEntity? offerRequestEntity = _offerRequestService.GetFirstOrDefault(x => x.RequestInfoId == requestInfo.Id && x.DemandOfferId == demandOfferEntity.Id).Data;
                        if (offerRequestEntity != null)
                        {
                            offerRequestViewModel.OfferRequestId = offerRequestEntity.Id;
                            offerRequestViewModel.Price = offerRequestEntity.UnitPrice ?? 0;
                            offerRequestViewModel.TotalPrice = offerRequestEntity.TotalPrice ?? 0;

                            offerRequestViewModels.Add(offerRequestViewModel);
                        }

                    }
                    demandOfferViewModel.RequestInfoViewModels = offerRequestViewModels;
                    demandViewModel.DemandOffers.Add(demandOfferViewModel);
                }
                List<CurrencyTypeEntity> currencyTypes = _currencyTypeService.GetAll().Data.ToList();
                ViewBag.CurrencyTypes = currencyTypes;
                List<CompanyLocation> locations = _companyLocationService.GetAll().Data.ToList();
                ViewBag.Locations = locations;
                List<Company> companies = _companyService.GetList().Data.ToList();
                ViewBag.Companies = companies;
                List<DepartmentEntity> departments = _departmentService.GetAll().Data.ToList();
                ViewBag.Departments = departments;
                List<ProviderEntity> providers = _providerService.GetAll().Data.ToList();
                ViewBag.Providers = providers;
                return View(demandViewModel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet("OfferPage")]
        public IActionResult OfferPage(long? DemandId, long? DemandOfferId)
        {
            List<RequestInfoEntity> requestInfos = _requestInfoService.GetList(x => x.DemandId == DemandId).Data.ToList();
            List<OfferRequestViewModel> offerRequestViewModels = new List<OfferRequestViewModel>();

            foreach (var requestInfo in requestInfos)
            {
                IDataResult<DemandOfferEntity> demandoffer = _demandOfferService.GetById((long)DemandOfferId);
                IDataResult<CurrencyTypeEntity> currencyType = _currencyTypeService.GetById((long)demandoffer.Data.CurrencyTypeId);
                OfferRequestViewModel offerRequestViewModel = new OfferRequestViewModel
                {
                    ProductCategoryId = requestInfo.ProductCategoryId,
                    RequestInfoId = requestInfo.Id,
                    DemandId = requestInfo.DemandId,
                    DemandOfferId = DemandOfferId.Value,
                    NebimCategoryId = requestInfo.NebimCategoryId,
                    NebimSubCategoryId = requestInfo.NebimSubCategoryId,
                    ProductName = requestInfo.ProductName,
                    ProductCode = requestInfo.ProductCode,
                    Quantity = requestInfo.Quantity,
                    Unit = requestInfo.Unit,
                    Currency = currencyType.Data.Symbol
                };

                OfferRequestEntity? offerRequestEntity = _offerRequestService.GetFirstOrDefault(x => x.RequestInfoId == requestInfo.Id && x.DemandOfferId == DemandOfferId.Value).Data;
                if (offerRequestEntity != null)
                {
                    offerRequestViewModel.OfferRequestId = offerRequestEntity.Id;
                    offerRequestViewModel.Price = offerRequestEntity.UnitPrice ?? 0;
                    offerRequestViewModel.TotalPrice = offerRequestEntity.TotalPrice ?? 0;

                    offerRequestViewModels.Add(offerRequestViewModel);
                }
                else if (requestInfo.IsFirst)
                {
                    offerRequestViewModels.Add(offerRequestViewModel);
                }

            }

            NebimConnection nebimConnection = new NebimConnection();
            var nebimCategoryModels = nebimConnection.GetNebimCategoryModels();
            offerRequestViewModels[0].NebimCategoryModels = nebimCategoryModels;

            var nebimSubcategoryModels = nebimConnection.GetNebimSubCategoryModels();
            offerRequestViewModels[0].NebimSubCategoryModels = nebimSubcategoryModels;

            var nebimProductModels = nebimConnection.GetNebimProductModels();
            offerRequestViewModels[0].NebimProductModels = nebimProductModels;

            List<ProductCategoryEntity> productCategories = _productCategoryService.GetAll().Data.ToList();
            offerRequestViewModels[0].ProductCategories = productCategories;

            return View(offerRequestViewModels);
        }
        [HttpPost("AddOfferRequest")]
        public IActionResult AddOfferRequest([FromForm] DemandViewModel demandViewModel)
        {
            #region UserIdentity
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.Claims;
            #endregion

            if (demandViewModel != null)
            {
                for (int i = 0; i < demandViewModel.ProductName.Count(); i++)
                {
                    var requestInfoId = demandViewModel.RequestInfoId[i];
                    if (requestInfoId != null && long.Parse(requestInfoId) > 0)
                    { }
                    else
                    {
                        var type = demandViewModel.Type[i];
                        var category = demandViewModel.Category[i];
                        var subcategory = demandViewModel.Subcategory[i];
                        var unit = demandViewModel.Unit[i];
                        var quantity = demandViewModel.Quantity[i];
                        var productname = demandViewModel.ProductName[i];
                        var productcode = demandViewModel.ProductCode[i];
                        var requestInfo = new RequestInfoEntity
                        {
                            DemandId = demandViewModel.DemandId.Value,
                            ProductCategoryId = Convert.ToInt32(type),
                            NebimCategoryId = Convert.ToInt32(category),
                            NebimSubCategoryId = Convert.ToInt32(subcategory),
                            Quantity = Convert.ToInt32(quantity),
                            ProductName = productname,
                            ProductCode = productcode,
                            Unit = unit,
                            IsDeleted = false,
                            IsFirst = false,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = null,
                            CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value),
                            UpdatedAt = null,
                        };

                        var requestInfoAdd = _requestInfoService.Add(requestInfo);

                        requestInfoId = requestInfoAdd.Id.ToString();
                    }
                    var totalPrice = demandViewModel.TotalPrice[i];
                    var price = demandViewModel.Price[i];

                    var offerRequest = new OfferRequestEntity
                    {
                        RequestInfoId = long.Parse(requestInfoId),
                        DemandOfferId = demandViewModel.DemandOfferId.Value,
                        IsDeleted = false,
                        Status = 0,
                        TotalPrice = decimal.Parse(totalPrice, CultureInfo.InvariantCulture),
                        UnitPrice = decimal.Parse(price.Replace('.', ',')),
                        CreatedDate = DateTime.Now,
                        UpdatedDate = null,
                        CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value),
                        UpdatedAt = null,
                    };

                    var offerRequestId = demandViewModel.OfferRequestId[i];
                    if (offerRequestId > 0)
                    {
                        offerRequest.Id = offerRequestId;

                        var offerRequestAdd = _offerRequestService.Update(offerRequest);
                    }
                    else
                    {
                        var offerRequestAdd = _offerRequestService.Add(offerRequest);
                    }
                }

                IDataResult<IList<OfferRequestEntity>> data = _offerRequestService.GetList(x => x.DemandOfferId == demandViewModel.DemandOfferId);
                if (data.Success)
                {
                    List<OfferRequestEntity> offerRequests = data.Data.ToList();
                    decimal sumTotalPrice = 0;
                    foreach (var offerRequest in offerRequests)
                    {
                        sumTotalPrice += offerRequest.TotalPrice.Value;
                    }

                    DemandOfferEntity demandOfferEntity = _demandOfferService.GetById(offerRequests[0].DemandOfferId).Data;
                    if (demandOfferEntity.IsNotNull())
                    {
                        demandOfferEntity.TotalPrice = sumTotalPrice;
                        _demandOfferService.Update(demandOfferEntity);
                    }
                }
            }

            return Ok();
        }
    }
}