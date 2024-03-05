using Demand.Business.Abstract.AuthorizationService;
using Demand.Business.Abstract.CompanyService;
using Demand.Business.Abstract.DemandService;
using Demand.Domain.Entities.Demand;
using Demand.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Demand.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthorizationService _authorizationService;
        public HomeController(ILogger<HomeController> logger, IAuthorizationService authorizationService)
        {
            _logger = logger;
            _authorizationService = authorizationService;
        }

        public IActionResult Index()
        {
            _authorizationService.Login();

            List<DemandEntity> demands = new List<DemandEntity>
        {
            new DemandEntity { Id = 1, CreatedAt = 1, CompanyLocationId = 1, CreatedDate = DateTime.Parse("01.01.2024"), Status = 0 }
        };

            return View(demands);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

    }
}
