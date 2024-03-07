using Demand.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demand.Presentation.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }


        public IActionResult Login()
        {
            return View();
        }


    }
}
