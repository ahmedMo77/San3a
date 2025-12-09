using San3a.Core.DTOs;

namespace San3a.Application.Interfaces
{
    public interface IAuthService
    {
        #region Methods
        Task<AuthResultDto> CreateAdminAsync(RegisterAdminDto dto);
        Task<AuthResultDto> RegisterCustomerAsync(RegisterAppUserDto dto);
        Task<AuthResultDto> RegisterCraftsmanAsync(RegisterCraftsmanDto dto);
        Task<AuthResultDto> LoginAsync(LoginDto dto);
        Task<AuthResultDto> RefreshTokenAsync(string refreshToken);
        #endregion
    }
}
