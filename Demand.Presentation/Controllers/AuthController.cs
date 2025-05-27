using Demand.Presentation.Utilities.Token;
using Microsoft.AspNetCore.Mvc;

namespace Demand.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;

        public AuthController(IConfiguration configuration, TokenService tokenService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }
        [HttpGet("Login")]
        public IActionResult Login(string SecretKey)
        {
            if (string.IsNullOrEmpty(SecretKey))
            {
                return BadRequest("Key is required");
            }
            else
            {
                string key = _configuration["Jwt:Key"];
                if (SecretKey == key)
                {
                    return Ok(_tokenService.CreateAccessTokenAsync().GetAwaiter().GetResult());
                }
                else
                {
                    return BadRequest("Ulaşım Key'i Hatalı");
                }
            }
        }

    }
}
