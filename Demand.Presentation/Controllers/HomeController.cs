using Demand.Business.Abstract.AuthorizationService;
using Demand.Business.Abstract.CompanyLocation;
using Demand.Business.Abstract.CompanyService;
using Demand.Business.Abstract.DemandProcessService;
using Demand.Business.Abstract.DemandService;
using Demand.Business.Abstract.Department;
using Demand.Business.Abstract.PersonnelService;
using Demand.Business.Concrete.DemandService;
using Demand.Core.Entities;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.CompanyLocation;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DemandProcess;
using Demand.Domain.Entities.DepartmentEntity;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.ViewModels;
using Demand.Infrastructure.DataAccess.Abstract.Department;
using Demand.Presentation.Models;
using Kep.Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Security.Claims;

namespace Demand.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDemandService _demandService;
        private readonly IPersonnelService _personnelService;
        private readonly ICompanyLocationService _companyLocationService;
        private readonly ICompanyService _companyService;
        private readonly IDepartmentService _departmentService;
        private readonly IDemandProcessService _demandProcessService;

        public HomeController(ILogger<HomeController> logger, IDemandService demandService, IPersonnelService personnelService, ICompanyLocationService companyLocationService, ICompanyService companyService, IDepartmentService departmentService, IDemandProcessService demandProcessService)
        {
            _logger = logger;
            _demandService = demandService;
            _personnelService = personnelService;
            _companyLocationService = companyLocationService;
            _companyService = companyService;
            _departmentService = departmentService;
            _demandProcessService = demandProcessService;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.Claims;
            var userId = long.Parse(claims.FirstOrDefault(x => x.Type == "UserId").Value);
            List<DemandViewModel> demandViewModels = new List<DemandViewModel>();

            List<DemandProcessEntity> demandProcesses = _demandProcessService.GetList(x => x.ManagerId == userId).Data.ToList();
            List<DemandEntity> DemandList = _demandService.GetList((x => x.CreatedAt == userId || demandProcesses.Select(d=> d.DemandId).Contains(x.Id))).Data.ToList();
            foreach (var demand in DemandList)
            {
                DemandViewModel viewModel = new DemandViewModel
                {
                    DemandId = demand.Id,
                    DemandDate = demand.CreatedDate,
                    Status = demand.Status,
                    DemandTitle=demand.DemandTitle,
                    CreatedAt = demand.CreatedAt
                };
                IDataResult<PersonnelEntity> personnelResult = _personnelService.GetById(demand.CreatedAt);
                if (personnelResult.IsNotNull())
                {
                    viewModel.DemanderName = personnelResult.Data.FirstName + " " + personnelResult.Data.LastName;
                }
                IDataResult<CompanyLocation> companyLocation = _companyLocationService.GetById(demand.CompanyLocationId);
                if (companyLocation.IsNotNull())
                {
                    viewModel.LocationName = companyLocation.Data.Name;
                }
                demandViewModels.Add(viewModel);

                List<Company> companies = _companyService.GetList().Data.ToList();
                ViewBag.Companies = companies;
                List<DepartmentEntity> departments = _departmentService.GetAll().Data.ToList();
                ViewBag.Department = departments;
            }
            return View(demandViewModels);

        }

        public IActionResult GetFilterData(int? status = null, long? locationId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            List<DemandViewModel> demandViewModels = new List<DemandViewModel>();
            List<DemandEntity> DemandList = _demandService.GetList(x =>
                                                                    (status == null || x.Status == status) &&
                                                                    (locationId == null || x.CompanyLocationId == locationId) &&
                                                                    (startDate == null || x.RequirementDate >= startDate) &&
                                                                    (endDate == null || x.RequirementDate <= endDate)).Data.ToList();
            foreach (var demand in DemandList)
            {
                DemandViewModel viewModel = new DemandViewModel
                {
                    DemandId = demand.Id,
                    DemandDate = demand.CreatedDate,
                    Status = demand.Status,
                };
                IDataResult<PersonnelEntity> personnelResult = _personnelService.GetById(demand.CreatedAt);
                if (personnelResult.IsNotNull())
                {
                    viewModel.DemanderName = personnelResult.Data.FirstName + " " + personnelResult.Data.LastName;
                }
                IDataResult<CompanyLocation> companyLocation = _companyLocationService.GetById(demand.CompanyLocationId);
                if (companyLocation.IsNotNull())
                {
                    viewModel.LocationName = companyLocation.Data.Name;
                }
                demandViewModels.Add(viewModel);
            }

            List<Company> companies = _companyService.GetList().Data.ToList();
            ViewBag.Companies = companies;
            List<DepartmentEntity> departments = _departmentService.GetAll().Data.ToList();
            ViewBag.Department = departments;

            return View(demandViewModels);
        }



        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            var result = _personnelService.GetByEmail(model.UserEmail);

            if (result.Success)
            {
                PersonnelEntity personnel = result.Data;

                if (personnel != null && personnel.Password == model.Password)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            // HTTP 400 - Bad Request durumu ve özel mesaj
            return StatusCode(400, new { success = false, message = "Kullanıcı adı veya şifre hatalı." });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
