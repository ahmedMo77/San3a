using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using San3a.Application.Interfaces;
using San3a.Core.DTOs;

namespace San3a.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminDto dto)
        {
            var result = await _authService.CreateAdminAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer(RegisterAppUserDto dto)
        {
            var result = await _authService.RegisterCustomerAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register-craftsman")]
        public async Task<IActionResult> RegisterCraftsman(RegisterCraftsmanDto dto)
        {
            var result = await _authService.RegisterCraftsmanAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = await _authService.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);

            return success ? Ok("Password changed successfully") : BadRequest("Failed to change password");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var token = await _authService.ForgotPasswordAsync(email);

            if (token == null)
                return NotFound("User not found");

            return Ok(new { ResetToken = token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var success = await _authService.ResetPasswordAsync(dto);

            if (!success)
                return BadRequest("Failed to reset password");

            return Ok("Password reset successfully");
        }
    }
}