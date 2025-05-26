using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventrack_backend_demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TwoFactorController : ControllerBase
    {
        private readonly ITwoFactorService _twoFactorService;

        public TwoFactorController(ITwoFactorService twoFactorService)
        {
            _twoFactorService = twoFactorService;
        }

        [HttpGet("setup")]
        public async Task<ActionResult<TwoFactorSetupDto>> GetSetup()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
                return Unauthorized();

            try
            {
                var setup = await _twoFactorService.GenerateSetupAsync(int.Parse(userId));
                return Ok(setup);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("enable")]
        public async Task<ActionResult> EnableTwoFactor(EnableTwoFactorDto enableDto)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _twoFactorService.EnableTwoFactorAsync(int.Parse(userId), enableDto.Code);
            if (!result)
            {
                return BadRequest("Invalid verification code");
            }

            return Ok(new { message = "Two-factor authentication enabled successfully" });
        }

        [HttpPost("disable")]
        public async Task<ActionResult> DisableTwoFactor()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _twoFactorService.DisableTwoFactorAsync(int.Parse(userId));
            if (!result)
            {
                return BadRequest("Failed to disable two-factor authentication");
            }

            return Ok(new { message = "Two-factor authentication disabled successfully" });
        }

        [HttpPost("verify")]
        public async Task<ActionResult> VerifyCode(VerifyTwoFactorDto verifyDto)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _twoFactorService.VerifyCodeAsync(int.Parse(userId), verifyDto.Code);
            if (!result)
            {
                return BadRequest("Invalid verification code");
            }

            return Ok(new { message = "Code verified successfully" });
        }
    }
}
