using San3a.Core.DTOs;
using San3a.Core.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> CreateAdminAsync(RegisterAdminDto dto);
        Task<AuthResultDto> RegisterCustomerAsync(RegisterAppUserDto dto);
        Task<AuthResultDto> RegisterCraftsmanAsync(RegisterCraftsmanDto dto);
        Task<AuthResultDto> LoginAsync(LoginDto dto);

        Task<AuthResultDto> RefreshTokenAsync(string refreshToken);

        Task<string> ForgotPasswordAsync(string email);
        Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    }
}
