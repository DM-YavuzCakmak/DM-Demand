using Demand.Business.Abstract.PersonnelService;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.ViewModels;
using Kep.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Demand.Presentation.Controllers
{
    [Route("Login")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IPersonnelService _personnelService;

        public LoginController(ILogger<LoginController> logger, IPersonnelService personnelService)
        {
            _logger = logger;
            _personnelService = personnelService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            var result = _personnelService.GetByEmail(model.UserEmail);

            if (result.Data.IsNotNull())
            {
                PersonnelEntity personnel = result.Data;

                if (personnel != null && personnel.Password == model.Password)
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false, message = "Kullanıcı adı veya şifre hatalı." });
        }
    }
}