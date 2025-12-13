using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using San3a.Application.Interfaces;
using San3a.Core.DTOs.auth;
using San3a.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace San3a.Application.Services
{
    public class TokenService : ITokenService
    {
        #region Fields
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        #endregion

        #region Constructors
        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }
        #endregion

        #region Public Methods
        public async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public RefreshToken CreateRefreshToken()
        {
            return new RefreshToken
            {
                Token = GenerateRandomToken(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            };
        }

        public async Task<AuthResultDto> GenerateTokensPairAsync(AppUser user)
        {
            var jwt = await GenerateJwtTokenAsync(user);
            var refreshToken = CreateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new AuthResultDto
            {
                Success = true,
                Token = jwt,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResultDto> RefreshTokenAsync(string token)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return new AuthResultDto { Success = false, Message = "Invalid token" };

            var rt = user.RefreshTokens.First(t => t.Token == token);

            if (!rt.IsActive)
                return new AuthResultDto { Success = false, Message = "Token expired or revoked" };

            rt.RevokedAt = DateTime.UtcNow;

            var newRt = CreateRefreshToken();
            user.RefreshTokens.Add(newRt);
            await _userManager.UpdateAsync(user);

            var jwt = await GenerateJwtTokenAsync(user);

            return new AuthResultDto
            {
                Success = true,
                Token = jwt,
                RefreshToken = newRt.Token
            };
        }
        #endregion

        #region Private Methods
        private string GenerateRandomToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
        #endregion
    }
}