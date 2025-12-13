using San3a.Core.DTOs.auth;
using San3a.Core.Entities;

namespace San3a.Application.Interfaces
{
    public interface ITokenService
    {
        #region Methods
        Task<string> GenerateJwtTokenAsync(AppUser user);
        RefreshToken CreateRefreshToken();
        Task<AuthResultDto> GenerateTokensPairAsync(AppUser user);
        Task<AuthResultDto> RefreshTokenAsync(string refreshToken);
        #endregion
    }
}