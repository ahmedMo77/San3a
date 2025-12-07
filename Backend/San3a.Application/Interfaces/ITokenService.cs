using San3a.Core.DTOs;
using San3a.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateJwtTokenAsync(AppUser user);
        RefreshToken CreateRefreshToken();
        Task<AuthResultDto> GenerateTokensPairAsync(AppUser user);
        Task<AuthResultDto> RefreshTokenAsync(string refreshToken);
    }
}