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
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> LoginAsync(LoginModel model);
        Task<JwtSecurityToken> GenerateJwtToken(AppUser user);
        Task<AuthModel> RefreshTokenAsync(string token);
    }
}
