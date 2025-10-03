using Demand.Business.Abstract.CompanyLocation;
using Demand.Business.Abstract.CompanyService;
using Demand.Business.Abstract.DemandOfferService;
using Demand.Business.Abstract.DemandProcessService;
using Demand.Business.Abstract.DemandService;
using Demand.Business.Abstract.Department;
using Demand.Business.Abstract.PersonnelRole;
using Demand.Business.Abstract.PersonnelService;
using Demand.Business.Abstract.ProductCategoryService;
using Demand.Core.Attribute;
using Demand.Core.DatabaseConnection.NebimConnection;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.CompanyLocation;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DemandOfferEntity;
using Demand.Domain.Entities.DemandProcess;
using Demand.Domain.Entities.DepartmentEntity;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Entities.PersonnelRole;
using Demand.Domain.Entities.ProductCategoryEntity;
using Demand.Domain.Enums;
using Demand.Domain.ViewModels;
using Demand.Presentation.Models;
using Kep.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
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
        private readonly IProductCategoryService _productCategoryService;
        private readonly IDemandOfferService _demandOfferService;
        private readonly IPersonnelRoleService _personnelRoleService;

        public HomeController(ILogger<HomeController> logger, IDemandService demandService, IPersonnelService personnelService, ICompanyLocationService companyLocationService, ICompanyService companyService, IDepartmentService departmentService, IDemandProcessService demandProcessService, IProductCategoryService productCategoryService, IDemandOfferService demandOfferService, IPersonnelRoleService personnelRoleService)
        {
            _logger = logger;
            _demandService = demandService;
            _personnelService = personnelService;
            _companyLocationService = companyLocationService;
            _companyService = companyService;
            _departmentService = departmentService;
            _demandProcessService = demandProcessService;
            _productCategoryService = productCategoryService;
            _demandOfferService = demandOfferService;
            _personnelRoleService = personnelRoleService;
        }

        #region Constants

        private static readonly long[] SpecialUsers = { 10, 57 };

        private static readonly long[] AllowedRoles =
        {
            (long)PersonnelRoleEnum.FinanceManagement,
            (long)PersonnelRoleEnum.HeadOfManager,
            (long)PersonnelRoleEnum.ITManager,
            (long)PersonnelRoleEnum.InsanKaynaklarıManager,
            (long)PersonnelRoleEnum.OperasyonManager,
            (long)PersonnelRoleEnum.HediyelikveGiftShopManager,
            (long)PersonnelRoleEnum.MimariManager,
            (long)PersonnelRoleEnum.SatınAlmaManager,
            (long)PersonnelRoleEnum.PazarlamaManager,
            (long)PersonnelRoleEnum.MuhasebeManager
        };

        #endregion

        #region Private Helpers

        private long GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            return long.Parse(claimsIdentity.Claims.First(x => x.Type == "UserId").Value);
        }

        private void SetViewBags()
        {
            ViewBag.Companies = _companyService.GetList().Data.ToList();
            ViewBag.Department = _departmentService.GetAll().Data.ToList();
            ViewBag.ProductCategories = _productCategoryService.GetAll().Data.ToList();
        }

        private bool HasAccess(long userId, PersonnelEntity personnel, List<PersonnelRoleEntity> roles,
                       List<DemandProcessEntity> demandProcesses, List<DemandProcessEntity> creatorDemandProcesses)
        {
            //if (demandProcesses.Count > 0 || userId == 10 || userId == 57 || personnelRoles.Any(x => x.RoleId == (int)PersonnelRoleEnum.FinanceManagement) || creatorDemandProcesses.Count > 0 || personnel.DepartmentId == (int)DepartmentEnum.Mimari)

            return demandProcesses.Any()
               || SpecialUsers.Contains(userId)
               || roles.Any(r => r.RoleId == (int)PersonnelRoleEnum.FinanceManagement)
               || creatorDemandProcesses.Any()
               || personnel.DepartmentId == (int)DepartmentEnum.Mimari;
        }

        private List<DemandEntity> GetDemandList(long userId, PersonnelEntity personnel, List<PersonnelRoleEntity> roles,
                                         List<DemandProcessEntity> demandProcesses)
        {
            var demandService = _demandService;

            if (personnel.DepartmentId == (int)DepartmentEnum.Mimari)
            {
                return demandService.GetList(x => x.DepartmentId == (int)DepartmentEnum.Mimari && !x.IsDeleted)
                                    .Data.OrderByDescending(t => t.CreatedDate).ToList();
            }

            if (roles.Any(r => r.RoleId == (int)PersonnelRoleEnum.FinanceManagement || r.RoleId == (int)PersonnelRoleEnum.HeadOfManager))
            {
                return demandService.GetList(x => !x.IsDeleted &&
                                                  (x.CreatedAt == userId
                                                   || x.Status == (int)DemandStatusEnum.approved
                                                   || demandProcesses.Select(d => d.DemandId).Contains(x.Id)))
                                    .Data.OrderByDescending(t => t.CreatedDate).ToList();
            }

            return demandService.GetList(x => !x.IsDeleted &&
                                              (x.CreatedAt == userId
                                               || SpecialUsers.Contains(userId)
                                               || demandProcesses.Select(d => d.DemandId).Contains(x.Id)))
                                .Data.OrderByDescending(t => t.CreatedDate).ToList();
        }

        private bool CanSeeDemand(long userId, PersonnelEntity personnel, List<PersonnelRoleEntity> roles,
                          DemandEntity demand, List<DemandProcessEntity> demandProcesses)
        {
            if (SpecialUsers.Contains(userId) || personnel.DepartmentId == (int)DepartmentEnum.Mimari)
                return true;

            var whoseTurnProcessList = _demandProcessService.GetList(x => x.DemandId == demand.Id && x.Status == 0).Data.ToList();

            return demand.CreatedAt == userId
                   || whoseTurnProcessList.Any(x => x.ManagerId == userId)
                   || roles.Any(r => AllowedRoles.Contains(r.RoleId));
        }

        private DemandViewModel BuildDemandViewModel(DemandEntity demand)
        {
            string whoseTurn = GetWhoseTurn(demand);
            var personnelResult = _personnelService.GetById(demand.CreatedAt);
            var demandOffers = _demandOfferService.GetList(x => x.DemandId == demand.Id).Data.ToList();

            var yk = _demandProcessService.GetList(x => x.DemandId == demand.Id).Data.OrderByDescending(x => x.Id).FirstOrDefault();
            var demandYk = _personnelService.GetById(yk.ManagerId).Data;

            bool cLevelParent = _demandProcessService.GetList(x => x.DemandId == demand.Id && x.Status == 0).Data.OrderBy(x => x.Id).FirstOrDefault()?.ManagerId
      == _demandProcessService.GetList(x => x.DemandId == demand.Id && x.Status == 0).Data.OrderByDescending(x => x.Id).FirstOrDefault()?.ManagerId;


            var viewModel = new DemandViewModel
            {
                DemandId = demand.Id,
                DemandDate = demand.CreatedDate,
                Status = demand.Status,
                DemandTitle = demand.DemandTitle,
                CreatedAt = demand.CreatedAt,
                WhoseTurn = whoseTurn,
                AprrovedDate = yk.UpdatedDate?.ToString("dd/MM/yyyy") ?? "TAMAMLANMADI",
                Yk = $"{demandYk.FirstName} {demandYk.LastName}",
                isDemandOffer = demandOffers.Any(),
                DemanderName = $"{personnelResult.Data.FirstName} {personnelResult.Data.LastName}",
                IsCLevel = cLevelParent
            };

            var companyLocation = _companyLocationService.GetById(demand.CompanyLocationId);
            if (companyLocation.IsNotNull())
                viewModel.LocationName = companyLocation.Data.Name;

            return viewModel;
        }

        private string GetWhoseTurn(DemandEntity demand)
        {
            var whoseTurnProcessList = _demandProcessService.GetList(x => x.DemandId == demand.Id && x.Status == 0).Data.ToList();

            if (whoseTurnProcessList.Any())
            {
                var process = whoseTurnProcessList.First();
                var personnel = _personnelService.GetById(process.ManagerId).Data;
                return demand.Status != (int)DemandStatusEnum.approved
                    ? $"{personnel.FirstName} {personnel.LastName}"
                    : "TAMAMLANDI";
            }

            var creator = _personnelService.GetById(demand.CreatedAt).Data;
            return demand.Status != (int)DemandStatusEnum.approved
                ? $"{creator.FirstName} {creator.LastName}"
                : "TAMAMLANDI";
        }

        private void AddNebimModels(DemandViewModel viewModel)
        {
            var nebimConnection = new NebimConnection();
            viewModel.NebimCategoryModels = nebimConnection.GetNebimCategoryModels();
            viewModel.NebimSubCategoryModels = nebimConnection.GetNebimSubCategoryModels();
            viewModel.NebimProductModels = nebimConnection.GetNebimProductModels();
        }

        #endregion

        [UserToken]
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Home";
            long userId = GetUserId();
            var personnel = _personnelService.GetById(userId).Data;
            var personnelRoles = _personnelRoleService.GetList(x => x.PersonnelId == personnel.Id).Data.ToList();

            SetViewBags();

            List<DemandProcessEntity> demandProcesses = _demandProcessService.GetList(x => x.ManagerId == userId).Data.ToList();
            List<DemandProcessEntity> creatorDemandProcesses = _demandProcessService.GetList(x => x.CreatedAt == userId).Data.ToList();

            List<DemandViewModel> demandViewModels = new List<DemandViewModel>();

            if (HasAccess(userId, personnel, personnelRoles, demandProcesses, creatorDemandProcesses))
            {
                var demandList = GetDemandList(userId, personnel, personnelRoles, demandProcesses);

                foreach (var demand in demandList)
                {
                    if (!CanSeeDemand(userId, personnel, personnelRoles, demand, demandProcesses))
                        continue;

                    demandViewModels.Add(BuildDemandViewModel(demand));
                }
            }

            if (!demandViewModels.Any())
                demandViewModels.Add(new DemandViewModel());

            AddNebimModels(demandViewModels[0]);

            return View(demandViewModels);
        }

        [UserToken]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login([FromQuery] string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
