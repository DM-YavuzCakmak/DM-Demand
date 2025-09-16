using Demand.Business.Abstract.AuthorizationService;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
    {
        var loginResult = _authorizationService.Login(loginViewModel);

        if (loginResult.Data == null)
            return Error("Hatalı email veya kullanıcı bulunamadı.");

        if (!VerifyPassword(loginResult.Data, loginViewModel.Password))
            return Error("Hatalı şifre.");

        var identity = CreateIdentity(loginResult.Data);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        var returnUrl = string.IsNullOrEmpty(loginViewModel.ReturnUrl)
            ? Url.Action("Index", "Home")
            : loginViewModel.ReturnUrl;

        return Success(returnUrl);
    }

    #region Private Helpers

    private bool VerifyPassword(PersonnelEntity user, string password)
    {
        var passwordHasher = new PasswordHasher<PersonnelEntity>();
        return passwordHasher.VerifyHashedPassword(user, user.Password, password)
               != PasswordVerificationResult.Failed;
    }

    private ClaimsIdentity CreateIdentity(PersonnelEntity user)
    {
        return new ClaimsIdentity(new[]
        {
            new Claim(ClaimConstants.UserId, user.Id.ToString()),
            new Claim(ClaimConstants.FirstName, user.FirstName),
            new Claim(ClaimConstants.LastName, user.LastName),
            new Claim(ClaimConstants.IsViewNewInvoice, user.IsViewNewInvoice.ToString())
        }, CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private JsonResult Success(string returnUrl) =>
        Json(new { success = true, returnUrl });

    private JsonResult Error(string message) =>
        Json(new { success = false, message });

    #endregion

    #region Constants

    private static class ClaimConstants
    {
        public const string UserId = "UserId";
        public const string FirstName = "FirstName";
        public const string LastName = "LastName";
        public const string IsViewNewInvoice = "IsViewNewInvoice";
    }

    #endregion
}
