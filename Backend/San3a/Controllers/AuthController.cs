using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using San3a.Application.Interfaces;
using San3a.Application.Services;
using San3a.Core.DTOs.auth;
namespace San3a.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : BaseApiController
    {
        #region Fields
        private readonly IAuthService _authService;
        #endregion

        #region Constructors
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region Public Methods

        #region Registration Endpoints
        [HttpPost("register/admin", Name = "RegisterAdmin")]
        [Authorize(Roles = "SuperAdmin")]
        [ProducesResponseType(typeof(AuthResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.CreateAdminAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register/customer", Name = "RegisterCustomer")]
        [AllowAnonymous]
      


        public async Task<IActionResult> RegisterCustomer([FromQuery] RegisterAppUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterCustomerAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register/craft", Name = "RegisterCraftsman")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterCraftsman([FromBody] RegisterCraftsmanDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterCraftsmanAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        #endregion

        #region Login Endpoints
        [HttpPost("login", Name = "Login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("refresh-token", Name = "RefreshToken")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest(new { Success = false, Message = "Refresh token is required" });

            var result = await _authService.RefreshTokenAsync(refreshToken);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("me", Name = "GetCurrentUser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetCurrentUser()
        {
            var userId = GetCurrentUserId();
            var email = GetCurrentUserEmail();
            var roles = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();

            return Ok(new
            {
                Success = true,
                UserId = userId,
                Email = email,
                Roles = roles
            });
        }
        #endregion

        #region Email Verification Endpoints
        [HttpPost("verify", Name = "VerifyEmail")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDTO dto)
        {
            var (success, message) = await _authService.VerifyEmailAsync(dto);

            if (!success)
                return BadRequest(message);

            return Ok(new { message = message });
        }

        [HttpPost("resend-verification", Name = "ResendVerification")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest("Email is required");

            var (success, message) = await _authService.ResendVerificationCodeAsync(request.Email);

            if (!success)
                return BadRequest(message);

            return Ok(new { message = message });
        }
        #endregion

        #region Password Reset Endpoints
        [HttpPost("forgot-password", Name = "ForgotPassword")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest("Email is required");

            var (success, message) = await _authService.RequestPasswordResetAsync(request.Email);

          
            return Ok(new { message = "If the email exists, a password reset code has been sent." });
        }

        [HttpPost("verify-reset-code", Name = "VerifyResetCode")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Code))
                return BadRequest("Email and code are required");

            var (success, message, resetToken) = await _authService.VerifyResetCodeAsync(request.Email, request.Code);

            if (!success)
                return BadRequest(message);

            return Ok(new
            {
                message = message,
                resetToken = resetToken
            });
        }
        [HttpPost("resend-reset-code", Name = "ResendResetCode")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResendResetCode([FromBody] ResendResetCodeRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest("Email is required");

            var (success, message) = await _authService.ResendPasswordResetCodeAsync(request.Email);

            if (!success)
                return BadRequest(message);

            return Ok(new { message = message });
        }
        [HttpPost("reset-password", Name = "ResetPassword")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
        
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            string? token = null;

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                token = authHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(token))
                return BadRequest("Reset token is required in Authorization header as Bearer token");

            if (string.IsNullOrEmpty(request.NewPassword))
                return BadRequest("New password is required");

            if (request.NewPassword != request.ConfirmPassword)
                return BadRequest("Passwords do not match");

            if (request.NewPassword.Length < 6)
                return BadRequest("Password must be at least 6 characters long");

            var (success, message) = await _authService.ResetPasswordAsync(token, request.NewPassword, request.ConfirmPassword);

            if (!success)
                return BadRequest(message);

            return Ok(new { message = "Password reset successfully. You can now sign in with your new password." });
        }
        #endregion

        #endregion
    }

 
   

   
}