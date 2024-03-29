﻿using Demand.Business.Abstract.CompanyLocation;
using Demand.Business.Abstract.CompanyService;
using Demand.Business.Abstract.CurrencyTypeService;
using Demand.Business.Abstract.DemandMediaService;
using Demand.Business.Abstract.DemandOfferService;
using Demand.Business.Abstract.DemandProcessService;
using Demand.Business.Abstract.DemandService;
using Demand.Business.Abstract.Department;
using Demand.Business.Abstract.PersonnelService;
using Demand.Business.Abstract.Provider;
using Demand.Business.Abstract.RequestInfo;
using Demand.Core.Utilities.Email;
using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.CompanyLocation;
using Demand.Domain.Entities.CurrencyTypeEntity;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DemandMediaEntity;
using Demand.Domain.Entities.DemandOfferEntity;
using Demand.Domain.Entities.DemandProcess;
using Demand.Domain.Entities.DepartmentEntity;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Entities.ProviderEntity;
using Demand.Domain.Entities.RequestInfoEntity;
using Demand.Domain.ViewModels;
using Kep.Helpers.Extensions;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
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

        public DemandsController(ILogger<HomeController> logger, IDemandService demandService, IDemandMediaService demandMediaService, IWebHostEnvironment webHostEnvironment, IDemandProcessService demandProcessService, ICompanyService companyService, IDepartmentService departmentService, IPersonnelService personnelService, ICompanyLocationService companyLocationService, IRequestInfoService requestInfoService, ICurrencyTypeService currencyTypeService, IDemandOfferService demandOfferService, IProviderService providerService)
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
                    RequestInfoId = demandOffer.RequestInfoId,
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
                demandViewModel.Material = requestInfos[0].ProductName;
                //demandViewModel.Quantity = requestInfos[0].Quantity;
                //demandViewModel.Unit = requestInfos[0].Unit;
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
                if (demandOfferEntity.SupplierId.HasValue)
                    demandOfferViewModel.SupplierId = demandOfferEntity.SupplierId.Value;
                if (!string.IsNullOrWhiteSpace(demandOfferEntity.SupplierName))
                    demandOfferViewModel.SupplierName = demandOfferEntity.SupplierName;

                ProviderEntity providerEntity = new ProviderEntity();
                if (providerEntities != null && providerEntities.Any())
                    providerEntity = providerEntities.Find(x => x.Id == demandOfferEntity.SupplierId);

                if (providerEntity != null)
                {
                    demandOfferViewModel.CompanyName = providerEntity.Name;
                    demandOfferViewModel.CompanyPhone = providerEntity.PhoneNumber;
                    demandOfferViewModel.CompanyAddress = providerEntity.Address;
                }
                demandOfferViewModel.RequestInfoId = demandOfferEntity.RequestInfoId;
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
                RequirementDate = DateTime.Now,/*(DateTime)demandViewModel.RequirementDate*/
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                UpdatedDate = null,
                CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value),
                UpdatedAt = null,
            };
            var addedDemand = _demandService.AddDemand(demandEntity);

            byte[]? file1 = null;
            byte[]? file2 = null;
            byte[]? file3 = null;
            string? file1FileName = demandViewModel.File1?.FileName;
            string? file2FileName = demandViewModel.File2?.FileName;
            string? file3FileName = demandViewModel.File3?.FileName;


            if (demandViewModel.File1 != null)
            {
                file1FileName = addedDemand.Id + "_" + demandViewModel.File1.FileName;
                file1 = GetFile(demandViewModel.File1);
            }
            if (demandViewModel.File2 != null)
            {
                file2FileName = addedDemand.Id + "_" + demandViewModel.File2.FileName;
                file2 = GetFile(demandViewModel.File2);
            }
            if (demandViewModel.File3 != null)
            {
                file3FileName = addedDemand.Id + "_" + demandViewModel.File3.FileName;
                file3 = GetFile(demandViewModel.File3);
            }

            if (demandViewModel.File1 != null)
            {
                DemandMediaEntity demandMediaEntity1 = SaveFileAndCreateEntity(demandViewModel.File1, addedDemand.Id);
                demandMediaEntity1.DemandId = addedDemand.Id;
                demandMediaEntity1.Path = demandMediaEntity1.Path;
                demandMediaEntity1.FileName = file1FileName;
                demandMediaEntity1.IsDeleted = false;
                demandMediaEntity1.CreatedDate = DateTime.Now;
                demandMediaEntity1.UpdatedDate = null;
                demandMediaEntity1.CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
                _demandMediaService.AddDemandMedia(demandMediaEntity1);
            }

            if (demandViewModel.File2 != null)
            {
                DemandMediaEntity demandMediaEntity2 = SaveFileAndCreateEntity(demandViewModel.File2, addedDemand.Id);
                demandMediaEntity2.DemandId = addedDemand.Id;
                demandMediaEntity2.Path = demandMediaEntity2.Path;
                demandMediaEntity2.FileName = file2FileName;
                demandMediaEntity2.IsDeleted = false;
                demandMediaEntity2.CreatedDate = DateTime.Now;
                demandMediaEntity2.UpdatedDate = null;
                demandMediaEntity2.CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
                _demandMediaService.AddDemandMedia(demandMediaEntity2);
            }

            if (demandViewModel.File3 != null)
            {
                DemandMediaEntity demandMediaEntity3 = SaveFileAndCreateEntity(demandViewModel.File3, addedDemand.Id);
                demandMediaEntity3.DemandId = addedDemand.Id;
                demandMediaEntity3.Path = demandMediaEntity3.Path;
                demandMediaEntity3.FileName = file3FileName;
                demandMediaEntity3.IsDeleted = false;
                demandMediaEntity3.CreatedDate = DateTime.Now;
                demandMediaEntity3.UpdatedDate = null;
                demandMediaEntity3.CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
                _demandMediaService.AddDemandMedia(demandMediaEntity3);
            }
            if (demandViewModel.Material.IsNotNull() && demandViewModel.Quantity.IsNotNull() && demandViewModel.Unit.IsNotNull())
            {
                RequestInfoEntity requestInfoEntity = new RequestInfoEntity();
                requestInfoEntity.DemandId = addedDemand.Id;
                requestInfoEntity.CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
                requestInfoEntity.CreatedDate = DateTime.Now;
                requestInfoEntity.IsDeleted = false;
                requestInfoEntity.UpdatedDate = null;
                requestInfoEntity.UpdatedAt = null;
                requestInfoEntity.ProductName = demandViewModel.Material;
                //requestInfoEntity.Quantity = demandViewModel.Quantity;
                //requestInfoEntity.Unit = demandViewModel.Unit;
                _requestInfoService.Add(requestInfoEntity);

            }
            if (demandViewModel.Material2.IsNotNull() && demandViewModel.Quantity2.IsNotNull() && demandViewModel.Unit2.IsNotNull())
            {
                RequestInfoEntity requestInfoEntity2 = new RequestInfoEntity();
                requestInfoEntity2.DemandId = addedDemand.Id;
                requestInfoEntity2.CreatedDate = DateTime.Now;
                requestInfoEntity2.CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
                requestInfoEntity2.IsDeleted = false;
                requestInfoEntity2.UpdatedDate = null;
                requestInfoEntity2.UpdatedAt = null;
                requestInfoEntity2.ProductName = demandViewModel.Material2;
                requestInfoEntity2.Quantity = demandViewModel.Quantity2;
                requestInfoEntity2.Unit = demandViewModel.Unit2;
                _requestInfoService.Add(requestInfoEntity2);
            }
            if (demandViewModel.Material3.IsNotNull() && demandViewModel.Quantity3.IsNotNull() && demandViewModel.Unit3.IsNotNull())
            {
                RequestInfoEntity requestInfoEntity3 = new RequestInfoEntity();
                requestInfoEntity3.DemandId = addedDemand.Id;
                requestInfoEntity3.CreatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
                requestInfoEntity3.CreatedDate = DateTime.Now;
                requestInfoEntity3.IsDeleted = false;
                requestInfoEntity3.UpdatedDate = null;
                requestInfoEntity3.UpdatedAt = null;
                requestInfoEntity3.ProductName = demandViewModel.Material3;
                requestInfoEntity3.Quantity = demandViewModel.Quantity3;
                requestInfoEntity3.Unit = demandViewModel.Unit3;
                _requestInfoService.Add(requestInfoEntity3);
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
                            string demandLink = "xxxxx";
                            var emailBody = $"Merhabalar Sayın " + parentPersonnel.FirstName + " " + parentPersonnel.LastName + ",<br/><br/>" +
                                        personnelEntity.FirstName + " " + personnelEntity.LastName + " tarafından," + demandEntity.DemandTitle + " başlıklı," + demandEntity.Id + " numaralı satın alma talebi açılmıştır. Aşağıdaki linkten talebi kontrol ederek onay vermenizi rica ederiz.<br/><br/>" +
                                        "Talep URL :" + demandLink + " <br/><br/>" +
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
                PersonnelEntity personnel = _personnelService.GetById(7).Data;

                string demandLink = "xxxxx";
                var emailBody = $"Merhabalar Sayın " + personnel.FirstName + " " + personnel.LastName + ",<br/><br/>" +
                            personnelEntity.FirstName + " " + personnelEntity.LastName + " tarafından," + demandEntity.DemandTitle + " başlıklı," + demandEntity.Id + " numaralı satın alma talebi açılmıştır. Aşağıdaki linkten talebi kontrol etmenizi rica ederiz.<br/><br/>" +
                            "Talep URL :" + demandLink + " <br/><br/>" +
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
                    PersonnelEntity personnel = _personnelService.GetById(7).Data;
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
                providerEntity.Name = updateDemandViewModel.Offer1CompanyName;
                providerEntity.PhoneNumber = updateDemandViewModel.Offer1CompanyPhone;
                providerEntity.Address = updateDemandViewModel.Offer1CompanyAddress;
                providerEntity.CreatedDate = DateTime.Now;
                providerEntity.CreatedAt = userId;
                providerEntity.UpdatedDate = null;
                providerEntity.UpdatedAt = null;
                _providerService.Add(providerEntity);
            }
            if (!updateDemandViewModel.IsProvider2Registered)
            {
                providerEntity2.Name = updateDemandViewModel.Offer2CompanyName;
                providerEntity2.PhoneNumber = updateDemandViewModel.Offer2CompanyPhone;
                providerEntity2.Address = updateDemandViewModel.Offer2CompanyAddress;
                providerEntity2.CreatedDate = DateTime.Now;
                providerEntity2.CreatedAt = userId;
                providerEntity2.UpdatedDate = null;
                providerEntity2.UpdatedAt = null;
                _providerService.Add(providerEntity2);
            }
            if (!updateDemandViewModel.IsProvider3Registered)
            {
                providerEntity3.Name = updateDemandViewModel.Offer3CompanyName;
                providerEntity3.PhoneNumber = updateDemandViewModel.Offer3CompanyPhone;
                providerEntity3.Address = updateDemandViewModel.Offer3CompanyAddress;
                providerEntity3.CreatedDate = DateTime.Now;
                providerEntity3.CreatedAt = userId;
                providerEntity3.UpdatedDate = null;
                providerEntity3.UpdatedAt = null;
                _providerService.Add(providerEntity3);

            }
            #endregion
            #region RequestInfo
            RequestInfoEntity requestInfo = _requestInfoService.GetByDemandId((long)updateDemandViewModel.DemandId).Data;
            if (updateDemandViewModel.Offer1CompanyName.IsNotNull())
            {
                DemandOfferEntity demandOfferEntity = new DemandOfferEntity();
                demandOfferEntity.CreatedAt = userId;
                demandOfferEntity.CreatedDate = DateTime.Now;
                demandOfferEntity.CurrencyTypeId = (long)updateDemandViewModel.Offer1CurrencyType;
                demandOfferEntity.DemandId = (long)updateDemandViewModel.DemandId;
                demandOfferEntity.SupplierName = updateDemandViewModel.Offer1CompanyName;
                demandOfferEntity.SupplierPhone = updateDemandViewModel.Offer1CompanyPhone;
                demandOfferEntity.SupplierId = updateDemandViewModel.Offer1CompanyId.IsNotNull() ? updateDemandViewModel.Offer1CompanyId.Value : providerEntity.Id;
                demandOfferEntity.IsDeleted = false;
                demandOfferEntity.RequestInfoId = requestInfo.Id;
                demandOfferEntity.Status = 0;
                demandOfferEntity.TotalPrice = updateDemandViewModel.Offer1TotalPrice;
                demandOfferEntity.UpdatedAt = null;
                demandOfferEntity.UpdatedDate = null;
                _demandOfferService.Add(demandOfferEntity);
            }
            if (updateDemandViewModel.Offer2CompanyName.IsNotNull())
            {
                DemandOfferEntity demandOfferEntity2 = new DemandOfferEntity();
                demandOfferEntity2.CreatedAt = userId;
                demandOfferEntity2.CreatedDate = DateTime.Now;
                demandOfferEntity2.CurrencyTypeId = (long)updateDemandViewModel.Offer2CurrencyType;
                demandOfferEntity2.DemandId = (long)updateDemandViewModel.DemandId;
                demandOfferEntity2.SupplierName = updateDemandViewModel.Offer2CompanyName;
                demandOfferEntity2.SupplierPhone = updateDemandViewModel.Offer2CompanyPhone;
                demandOfferEntity2.SupplierId = updateDemandViewModel.Offer2CompanyId.IsNotNull() ? updateDemandViewModel.Offer2CompanyId.Value : providerEntity2.Id;
                demandOfferEntity2.IsDeleted = false;
                demandOfferEntity2.RequestInfoId = requestInfo.Id;
                demandOfferEntity2.Status = 0;
                demandOfferEntity2.TotalPrice = updateDemandViewModel.Offer2TotalPrice;
                demandOfferEntity2.UpdatedAt = null;
                demandOfferEntity2.UpdatedDate = null;
                _demandOfferService.Add(demandOfferEntity2);
            }
            if (updateDemandViewModel.Offer3CompanyName.IsNotNull())
            {
                DemandOfferEntity demandOfferEntity3 = new DemandOfferEntity();
                demandOfferEntity3.CreatedAt = userId;
                demandOfferEntity3.CreatedDate = DateTime.Now;
                demandOfferEntity3.CurrencyTypeId = (long)updateDemandViewModel.Offer3CurrencyType;
                demandOfferEntity3.DemandId = (long)updateDemandViewModel.DemandId;
                demandOfferEntity3.SupplierName = updateDemandViewModel.Offer3CompanyName;
                demandOfferEntity3.SupplierPhone = updateDemandViewModel.Offer3CompanyPhone;
                demandOfferEntity3.SupplierId = updateDemandViewModel.Offer3CompanyId.IsNotNull() ? updateDemandViewModel.Offer3CompanyId.Value : providerEntity3.Id;
                demandOfferEntity3.IsDeleted = false;
                demandOfferEntity3.RequestInfoId = requestInfo.Id;
                demandOfferEntity3.Status = 0;
                demandOfferEntity3.TotalPrice = updateDemandViewModel.Offer3TotalPrice;
                demandOfferEntity3.UpdatedAt = null;
                demandOfferEntity3.UpdatedDate = null;
                _demandOfferService.Add(demandOfferEntity3);
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
    }
}
