using Demand.Business.Abstract.AuthorizationService;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Demand.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationsController : Controller
{
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationsController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel )
    {
        //var aaaa = (ClaimsIdentity)User.Identity;
        //var claims = aaaa.Claims;

        var dataResult = _authorizationService.Login(loginViewModel);
        if (dataResult.Data == null)
        {
            return Json(new { success = false, message = "Hatalı email veya kullanıcı bulunamadı." });
        }

         #region HashingChecks
        Microsoft.AspNetCore.Identity.PasswordHasher<PersonnelEntity> passwordHasher = new();
        var resultant = passwordHasher.VerifyHashedPassword(dataResult.Data, dataResult.Data.Password, loginViewModel.Password);
        #endregion
        if (resultant == PasswordVerificationResult.Failed) 
        {
            return Json(new { success = false, message = "Hatalı şifre." });
        }

        var identity = new System.Security.Claims.ClaimsIdentity(new[]
        {
    new System.Security.Claims.Claim("UserId", dataResult.Data.Id.ToString()),
    new System.Security.Claims.Claim("FirstName", dataResult.Data.FirstName.ToString()),
    new System.Security.Claims.Claim("LastName", dataResult.Data.LastName.ToString())
}, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                       new(identity),
                                       new());

        if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl))
        {
            //return Redirect(loginViewModel.ReturnUrl);
            return Json(new { success = true, returnUrl = loginViewModel.ReturnUrl });
        }
        return Json(new { success = true, returnUrl = Url.Action("Index", "Home") });

        return Json(new { success = true });
    }
}
