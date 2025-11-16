using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using San3a.Application.Interfaces;
using San3a.Core.DTOs;
using San3a.Core.Entities;
using San3a.Core.Enums;
using San3a.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;


        public AuthService(UserManager<AppUser> userManager, AppDbContext context, IMapper mapper,
            IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _config = config;
            _roleManager = roleManager;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            var userExist = await _userManager.FindByEmailAsync(model.Email);

            if (userExist != null)
                return new AuthModel { Errors = { "User already exists" } };

            var user = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                errors.Insert(0, "User creation failed:");
                return new AuthModel { Errors = errors };
            }

            var role = model.Role;
            if (!await IsRoleAllowed(role))
                return new AuthModel { Errors = { "Role does not exist." } };

            await _userManager.AddToRoleAsync(user, role);

            var jwtSecurityToken = await GenerateJwtToken(user);

            var refreshToken = GenerateRefreshToken();

            var authModel = new AuthModel
            {
                FullName = user.FullName,
                Email = user.Email,
                Role = model.Role,
                Succeeded = true,
                jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresAt
            };

            return authModel;
        }

        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            var user =await _userManager.FindByEmailAsync(model.Email);
            if(user == null || ! await _userManager.CheckPasswordAsync(user, model.Password))
                return new AuthModel { Errors = { "Invalid credentials!" } };

            var jwtSecurityToken = await GenerateJwtToken(user);

            var authModel = new AuthModel
            {
                Succeeded = true,
                jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };

            if(user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.First(t => t.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresAt;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresAt;
            }

            return authModel;
        }

        public async Task<JwtSecurityToken> GenerateJwtToken(AppUser user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            { 
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }


            var jwtSettings = _config.GetSection("JWT");

            var signingKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var signingCred = new SigningCredentials
                (signingKey, SecurityAlgorithms.HmacSha256);

            var jwtTken = new JwtSecurityToken
                (
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: userClaims,
                    expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["DurationInMinutes"])),
                    signingCredentials: signingCred
                );
            return jwtTken;
        }

        public async Task<AuthModel> RefreshTokenAsync(string token)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(r => r.Token == token));

            if (user == null)
                return new AuthModel { Errors = { "Invalid token" } };

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if(!refreshToken.IsActive)
                return new AuthModel { Errors = { "Inactive token!" } };

            // Revoke Refresh toke
            refreshToken.RevokedAt = DateTime.UtcNow;

            // generate new refresh token
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            // generate new jwt token
            var jwtSecurityToken = await GenerateJwtToken(user);

            var authModel = new AuthModel()
            {
                Succeeded = true,
                jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresAt
            };

            return authModel;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
        }
        private async Task<bool> IsRoleAllowed(string role)
        {
            var allowedRoles = new List<string>
            {
                UserType.Worker.ToString(),
                UserType.Customer.ToString()
            };

            if (!allowedRoles.Contains(role))
                return false;
            
            return true;
        }
        private async Task<bool> IsApprovedUser(AppUser user)
        {
            if(await _userManager.IsInRoleAsync(user,UserType.Customer.ToString()))
                return true;

            return false;
        }
    }
}
