using AutoMapper;
using Microsoft.AspNetCore.Identity;
using San3a.Application.Interfaces;
using San3a.Core.DTOs;
using San3a.Core.Entities;
using San3a.Infrastructure.Data;
using System.Data;

namespace San3a.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService, AppDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _context = context;
            _mapper = mapper;
        }


        // --------------------
        // Create Admin
        // --------------------
        public async Task<AuthResultDto> CreateAdminAsync(RegisterAdminDto dto)
        {
            var user = _mapper.Map<AppUser>(dto);
            var result = await _userManager.CreateAsync(user, dto.Password);
            
            if (!result.Succeeded)
            {   return new AuthResultDto
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            var role = dto.IsSuperAdmin ? "SuperAdmin" : "Admin";

            await _userManager.AddToRoleAsync(user, role);
            var admin = new Admin
            {
                Id = user.Id,
                AppUser = user,
                IsSuperAdmin = dto.IsSuperAdmin
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
            return await _tokenService.GenerateTokensPairAsync(user);
        }
        // --------------------
        // Register Customer
        // --------------------
        public async Task<AuthResultDto> RegisterCustomerAsync(RegisterAppUserDto dto)
        {
            var user = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {   return new AuthResultDto
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            await _userManager.AddToRoleAsync(user, "Customer");

            var customer = new Customer 
            { 
                Id = user.Id,
                AppUser = user 
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return await _tokenService.GenerateTokensPairAsync(user);
        }

        // --------------------
        // Register Craftsman
        // --------------------
        public async Task<AuthResultDto> RegisterCraftsmanAsync(RegisterCraftsmanDto dto)
        {
            var user = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {  return new AuthResultDto
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            await _userManager.AddToRoleAsync(user, "Craftsman");

            var craftsman = new Craftsman
            {
                Id = user.Id,
                AppUser = user,
                NationalId = dto.NationalId,
                ServiceId = dto.ServiceId,
                IsVerified = false
            };

            _context.Craftsmen.Add(craftsman);
            await _context.SaveChangesAsync();
            return await _tokenService.GenerateTokensPairAsync(user);
        }

        // --------------------
        // Login
        // --------------------
        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return new AuthResultDto { Success = false, Message = "Invalid credentials" };

            return await _tokenService.GenerateTokensPairAsync(user);
        }

        // Refresh Token
        public Task<AuthResultDto> RefreshTokenAsync(string refreshToken)
        {
            return _tokenService.RefreshTokenAsync(refreshToken);
        }


        // Password Services

        public async Task<bool> ChangePasswordAsync
            (string userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return false;

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            return result.Succeeded;
        }
    }
}
