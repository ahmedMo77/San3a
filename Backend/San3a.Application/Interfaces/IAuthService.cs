using San3a.Core.DTOs.auth;

namespace San3a.Application.Interfaces
{
    public interface IAuthService
    {
        #region Registration Methods
        Task<AuthResultDto> CreateAdminAsync(RegisterAdminDto dto);
        Task<AuthResultDto> RegisterCustomerAsync(RegisterAppUserDto dto);
        Task<AuthResultDto> RegisterCraftsmanAsync(RegisterCraftsmanDto dto);
        #endregion

        #region Login Methods
        Task<AuthResultDto> LoginAsync(LoginDto dto);
        Task<AuthResultDto> RefreshTokenAsync(string refreshToken);
        #endregion

        #region Email Verification Methods
        Task<(bool Success, string Message)> VerifyEmailAsync(VerifyEmailDTO dto);
        Task<(bool Success, string Message)> ResendVerificationCodeAsync(string email);
        Task<(bool Success, string Message)> ResendPasswordResetCodeAsync(string email);
        #endregion

        #region Password Reset Methods
        Task<(bool Success, string Message)> RequestPasswordResetAsync(string email);
        Task<(bool Success, string Message, string? ResetToken)> VerifyResetCodeAsync(string email, string code);
        Task<(bool Success, string Message)> ResetPasswordAsync(string token, string newPassword, string confirmPassword);
        #endregion
    }
}