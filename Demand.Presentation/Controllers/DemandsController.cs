using Demand.Business.Abstract.CompanyLocation;
using Demand.Business.Abstract.CompanyService;
using Demand.Business.Abstract.DemandMediaService;
using Demand.Business.Abstract.DemandProcessService;
using Demand.Business.Abstract.DemandService;
using Demand.Business.Abstract.Department;
using Demand.Business.Abstract.PersonnelService;
using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.CompanyLocation;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DemandMediaEntity;
using Demand.Domain.Entities.DemandProcess;
using Demand.Domain.Entities.DepartmentEntity;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.ViewModels;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
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

        public DemandsController(ILogger<HomeController> logger, IDemandService demandService, IDemandMediaService demandMediaService, IWebHostEnvironment webHostEnvironment, IDemandProcessService demandProcessService, ICompanyService companyService, IDepartmentService departmentService, IPersonnelService personnelService, ICompanyLocationService companyLocationService)
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
        }

        public IActionResult Detail(long id)
        {
            DemandEntity demand = _demandService.GetById(id).Data;
            List<DemandMediaEntity> demandMediaEntities = _demandMediaService.GetByDemandId(id).ToList();
            CompanyLocation companyLocation = _companyLocationService.GetById(demand.CompanyLocationId).Data;
            Company company = _companyService.GetById(companyLocation.CompanyId).Data;
            PersonnelEntity personnel = _personnelService.GetById(demand.CreatedAt).Data;
            DepartmentEntity department = _departmentService.GetById(demand.DepartmentId).Data;
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

            List<Company> companies = _companyService.GetList().Data.ToList();
            ViewBag.Companies = companies;
            List<DepartmentEntity> departments = _departmentService.GetAll().Data.ToList();
            ViewBag.Department = departments;

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

            PersonnelEntity personnelEntity = _personnelService.GetById(long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value)).Data;

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
                    //Send Mail
                }

                if (parentPersonnel.ParentId != null)
                    parentPersonnel = _personnelService.GetById((long)parentPersonnel.ParentId).Data;
                else
                    break;
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
                    //Send Mail nextDemandProcessEntity
                }
                else
                {
                    DemandEntity demandEntity = _demandService.GetById(demandStatusChangeViewModel.DemandId).Data;
                    demandEntity.Status = 2;
                    demandEntity.UpdatedAt = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
                    demandEntity.UpdatedDate = DateTime.Now;

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
                    //Send Mail previousDemandProcessList
                }

                // Send Mail demandProcessEntity.CreatedAt (Açan Kişiye de iptal Bildirimi için)
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
                    Path = filePath
                };
            }

            return null;
        }

        private void AddDemandProcess()
        {

        }

    }
}
