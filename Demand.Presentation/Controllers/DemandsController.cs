using Demand.Business.Abstract.CompanyLocation;
using Demand.Business.Abstract.CompanyService;
using Demand.Business.Abstract.CurrencyTypeService;
using Demand.Business.Abstract.DemandMediaService;
using Demand.Business.Abstract.DemandOfferService;
using Demand.Business.Abstract.DemandProcessService;
using Demand.Business.Abstract.DemandService;
using Demand.Business.Abstract.Department;
using Demand.Business.Abstract.OfferRequestService;
using Demand.Business.Abstract.PersonnelService;
using Demand.Business.Abstract.Provider;
using Demand.Business.Abstract.RequestInfo;
using Demand.Core.DatabaseConnection.NebimConnection;
using Demand.Core.Utilities.Email;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.CompanyLocation;
using Demand.Domain.Entities.CurrencyTypeEntity;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DemandMediaEntity;
using Demand.Domain.Entities.DemandOfferEntity;
using Demand.Domain.Entities.DemandProcess;
using Demand.Domain.Entities.DepartmentEntity;
using Demand.Domain.Entities.OfferRequestEntity;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Entities.ProviderEntity;
using Demand.Domain.Entities.RequestInfoEntity;
using Demand.Domain.ViewModels;
using Kep.Helpers.Extensions;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;


namespace Demand.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        public DemandsController(ILogger<HomeController> logger, IDemandService demandService, IDemandMediaService demandMediaService, IWebHostEnvironment webHostEnvironment, IDemandProcessService demandProcessService, ICompanyService companyService, IDepartmentService departmentService, IPersonnelService personnelService, ICompanyLocationService companyLocationService, IRequestInfoService requestInfoService, ICurrencyTypeService currencyTypeService, IDemandOfferService demandOfferService, IProviderService providerService, IOfferRequestService offerRequestService)
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
        }

        public IActionResult Detail(long id)
        {
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
                DepartmentName = department.Name
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

            DemandEntity demand = _demandService.GetById(id).Data;
            List<DemandMediaEntity> demandMediaEntities = _demandMediaService.GetByDemandId(id).ToList();
            CompanyLocation companyLocation = _companyLocationService.GetById(demand.CompanyLocationId).Data;
            Company company = _companyService.GetById(companyLocation.CompanyId).Data;
            PersonnelEntity personnel = _personnelService.GetById(demand.CreatedAt).Data;
            DepartmentEntity department = _departmentService.GetById(demand.DepartmentId).Data;
            List<RequestInfoEntity> requestInfos = _requestInfoService.GetList(x => x.DemandId == id).Data.ToList();
            List<DemandOfferEntity> demandOfferEntities = _demandOfferService.GetList(x => x.DemandId == id).Data.ToList();
            List<long> supplierIds = new List<long>();
            supplierIds = demandOfferEntities.Select(x => x.SupplierId.Value).ToList();
            List<ProviderEntity> providerEntities = _providerService.GetList(x => supplierIds.Contains(x.Id)).Data.ToList();
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
                DepartmentName = department.Name
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
                DemandTitle = demandViewModel.DemandTitle,
                DepartmentId = (long)demandViewModel.DepartmentId,
                Status = 0,
                Description = demandViewModel.Description,
                RequirementDate =(DateTime)demandViewModel.RequirementDate,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                UpdatedDate = null,
                CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value),
                UpdatedAt = null,
            };
            var addedDemand = _demandService.AddDemand(demandEntity);

            for (int i = 0; i < demandViewModel.Category.Count()-1; i++)
            {
                var category = demandViewModel.Category[i] == "Lütfen seçiniz" ? null : demandViewModel.Category[i];
                var subcategory = demandViewModel.Subcategory[i] ==null ? null : demandViewModel.Subcategory[i];
                var unit = demandViewModel.Unit[i];
                var quantity = demandViewModel.Quantity[i];
                var productname = demandViewModel.ProductName[i];
                var productcode = demandViewModel.ProductCode[i];
                var requestInfo = new RequestInfoEntity
                {
                    DemandId = addedDemand.Id,
                    ProductCategoryId = Convert.ToInt32(category),
                    ProductSubCategoryId = Convert.ToInt32(subcategory),
                    Quantity = Convert.ToInt32(quantity),
                    ProductName = productname,
                    ProductCode = productcode,
                    Unit = unit,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = null,
                    CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value),
                    UpdatedAt = null,
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

                    if (parentPersonnel.ParentId != null)
                        parentPersonnel = _personnelService.GetById((long)parentPersonnel.ParentId).Data;
                    else
                        break;
                }
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

            DemandProcessEntity? demandProcessEntity = demandProcessEntities.FirstOrDefault(x => x.ManagerId == long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value));
            if (demandProcessEntity == null)
            {
                return BadRequest("Talebe Ait Durum Değiştirme Yetkiniz Bulunmamaktadır.");
            }

            if (demandProcessEntity.Status != 0)
            {
                return BadRequest("Talep Durumu Değiştirmeye Uygun Değildir.");
            }

            demandProcessEntity.Status = demandStatusChangeViewModel.Status;
            demandProcessEntity.Desciription = demandStatusChangeViewModel.Description ?? string.Empty;
            demandProcessEntity.UpdatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
            demandProcessEntity.UpdatedDate = DateTime.Now;

            _demandProcessService.UpdateDemandProcess(demandProcessEntity);

            if (demandProcessEntity.Status == 2)//Approve
            {
                DemandProcessEntity? nextDemandProcessEntity = demandProcessEntities.FirstOrDefault(x => x.HierarchyOrder == demandProcessEntity.HierarchyOrder + 1);
                if (nextDemandProcessEntity != null)
                {
                    PersonnelEntity personnel = _personnelService.GetById(nextDemandProcessEntity.ManagerId).Data;
                    PersonnelEntity demandOpenPerson = _personnelService.GetById(demandProcessEntity.CreatedAt).Data;
                    DemandEntity demand = _demandService.GetById(demandProcessEntity.DemandId).Data;

                    string demandLink = "xxxxx";
                    var emailBody = $"Merhabalar Sayın " + personnel.FirstName + " " + personnel.LastName + ",<br/><br/>" +
                                demandOpenPerson.FirstName + " " + demandOpenPerson.LastName + " tarafından," + demand.DemandTitle + " başlıklı," + demand.Id + " numaralı satın alma talebi açılmıştır. Aşağıdaki linkten talebi kontrol ederek onay vermenizi rica ederiz.<br/><br/>" +
                                "Talep URL :" + demandLink + " <br/><br/>" +
                                "Saygılarımızla.";
                    EmailHelper.SendEmail(new List<string> { personnel.Email }, "Onayınızı Bekleyen Satın Alma Talebi", emailBody);
                }

                else
                {
                    DemandEntity demandEntity = _demandService.GetById(demandStatusChangeViewModel.DemandId).Data;
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

                    string demandLink = "xxxxx";
                    var emailBody = $"Merhabalar Sayın " + personnel.FirstName + " " + personnel.LastName + ",<br/><br/>" +
                                demandOpenPerson.FirstName + " " + demandOpenPerson.LastName + " tarafından," + demandEntity.DemandTitle + " başlıklı," + demandEntity.Id + " numaralı satın alma talebi onaylanmıştır.Bilginize sunarız.<br/><br/>" +
                                "Saygılarımızla.";
                    EmailHelper.SendEmail(new List<string> { personnel.Email }, "Onayınızı Bekleyen Satın Alma Talebi", emailBody);













                    _demandService.Update(demandEntity);
                }
            }
            else if (demandProcessEntity.Status == 1)//Cancelled
            {
                DemandEntity demandEntity = _demandService.GetById(demandStatusChangeViewModel.DemandId).Data;
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
                string uniqueFileName = demandId + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

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
                demandOfferEntity.Status = 0;
                demandOfferEntity.TotalPrice = updateDemandViewModel.OfferTotalPrice;
                demandOfferEntity.UpdatedAt = null;
                demandOfferEntity.UpdatedDate = null;
                var demandOfferAdd = _demandOfferService.Add(demandOfferEntity);
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
            NebimConnection nebimConnection = new NebimConnection();
            nebimConnection.GetNebimCategoryModels();

            DemandEntity demand = _demandService.GetById(DemandId).Data;
            List<DemandMediaEntity> demandMediaEntities = _demandMediaService.GetByDemandId(DemandId).ToList();
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
                ConfirmingNote = demandProcess.IsNotNull() ? demandProcess.Desciription :""
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
            demandViewModel.DemandOffers = new List<DemandOfferViewModel>();
            foreach (var demandOfferEntity in demandOfferEntities.OrderBy(x => x.Id).ToList())
            {
                DemandOfferViewModel demandOfferViewModel = new DemandOfferViewModel();
                demandOfferViewModel.DemandOfferId = demandOfferEntity.Id;
                demandOfferViewModel.Status = demandOfferEntity.Status;
                demandOfferViewModel.TotalPrice = demandOfferEntity.TotalPrice;
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
                        DemandId = requestInfo.DemandId,
                        DemandOfferId = demandOfferEntity.Id,
                        ProductCategoryId = requestInfo.ProductCategoryId,
                        ProductSubCategoryId = requestInfo.ProductSubCategoryId,
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
                    RequestInfoId = requestInfo.Id,
                    DemandId = requestInfo.DemandId,
                    DemandOfferId = DemandOfferId.Value,
                    ProductCategoryId = requestInfo.ProductCategoryId,
                    ProductSubCategoryId = requestInfo.ProductSubCategoryId,
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
                }

                offerRequestViewModels.Add(offerRequestViewModel);
            }

            NebimConnection nebimConnection = new NebimConnection();
            var nebimCategoryModels = nebimConnection.GetNebimCategoryModels();
            offerRequestViewModels[0].NebimCategoryModels = nebimCategoryModels;

            var nebimSubcategoryModels = nebimConnection.GetNebimSubCategoryModels();
            offerRequestViewModels[0].NebimSubCategoryModels = nebimSubcategoryModels;

            var nebimProductModels = nebimConnection.GetNebimProductModels();
            offerRequestViewModels[0].NebimProductModels = nebimProductModels;

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
                for (int i = 0; i < demandViewModel.Category.Count(); i++)
                {
                    var requestInfoId = demandViewModel.RequestInfoId[i];
                    if (requestInfoId != null && long.Parse(requestInfoId) > 0)
                    { }
                    else
                    {
                        var category = demandViewModel.Category[i];
                        var subcategory = demandViewModel.Subcategory[i];
                        var unit = demandViewModel.Unit[i];
                        var quantity = demandViewModel.Quantity[i];
                        var productname = demandViewModel.ProductDescription[i];
                        var productcode = demandViewModel.ProductCode[i];
                        var requestInfo = new RequestInfoEntity
                        {
                            DemandId = demandViewModel.DemandId.Value,
                            ProductCategoryId = Convert.ToInt32(category),
                            ProductSubCategoryId = Convert.ToInt32(subcategory),
                            Quantity = Convert.ToInt32(quantity),
                            ProductName = productname,
                            ProductCode = productcode,
                            Unit = unit,
                            IsDeleted = false,
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
                        TotalPrice = decimal.Parse(totalPrice.Replace('.', ',')),
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
            }

            return Ok();
        }
    }
}