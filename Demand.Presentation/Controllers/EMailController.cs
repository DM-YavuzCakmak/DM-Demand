using Demand.Domain.Entities.FormDataEntity;
using Demand.Presentation.Services;
using Demand.Presentation.Utilities.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demand.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EMailController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly EMailService _emailService;

        public EMailController(EMailService emailService, TokenService tokenService)
        {
            _emailService = emailService;
            _tokenService = tokenService;
        }

        //[Authorize]
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] FormData formData)
        {
            var success = await _emailService.SendEmailAsync(formData);
            if (success)
                return Ok(new { success = true });
            else
                return StatusCode(500, new { success = false, message = "Mail gönderilemedi." });
        }
    }
}
