using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using San3a.Application.Interfaces;
using San3a.Core.DTOs;
using San3a.Core.Entities;
using San3a.Infrastructure.Data;

namespace San3a.Application.Services
{
    public class AuthService : IAuthService
    {
        #region Fields
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService, AppDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods
        public async Task<AuthResultDto> CreateAdminAsync(RegisterAdminDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = "Password and Confirm Password do not match"
                };
            }

            var user = _mapper.Map<AppUser>(dto);
            var result = await _userManager.CreateAsync(user, dto.Password);
            
            if (!result.Succeeded)
            {   
                return new AuthResultDto
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

        public async Task<AuthResultDto> RegisterCustomerAsync(RegisterAppUserDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = "Password and Confirm Password do not match"
                };
            }

            // Check if NationalId already exists (if provided)
            if (!string.IsNullOrWhiteSpace(dto.NationalId))
            {
                var existingCustomer = _context.Customers.FirstOrDefault(c => c.NationalId == dto.NationalId);
                if (existingCustomer != null)
                {
                    return new AuthResultDto
                    {
                        Success = false,
                        Message = "National ID already exists"
                    };
                }
            }

            var user = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {   
                return new AuthResultDto
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            await _userManager.AddToRoleAsync(user, "Customer");

            var customer = new Customer 
            { 
                Id = user.Id,
                AppUser = user,
                NationalId = dto.NationalId
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return await _tokenService.GenerateTokensPairAsync(user);
        }

        public async Task<AuthResultDto> RegisterCraftsmanAsync(RegisterCraftsmanDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = "Password and Confirm Password do not match"
                };
            }

            // Check if NationalId already exists
            var existingCraftsman = _context.Craftsmen.FirstOrDefault(c => c.NationalId == dto.NationalId);
            if (existingCraftsman != null)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = "National ID already exists"
                };
            }

            var user = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {  
                return new AuthResultDto
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
            
            var authResult = await _tokenService.GenerateTokensPairAsync(user);
            authResult.Message = "Registration successful. Your account is pending admin verification.";
            return authResult;
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            AppUser user = null;
            
            // Try to find user by email first
            user = await _userManager.FindByEmailAsync(dto.EmailOrUsername);
            
            // If not found by email, try by username
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(dto.EmailOrUsername);
            }
            
            // If not found by username, try by full name
            if (user == null)
            {
                user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.FullName == dto.EmailOrUsername);
            }
            
            // Validate user and password
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return new AuthResultDto { Success = false, Message = "Invalid credentials" };

            // Check if user is a craftsman and if verified
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Craftsman"))
            {
                var craftsman = await _context.Craftsmen.FindAsync(user.Id);
                if (craftsman != null && !craftsman.IsVerified)
                {
                    var result = await _tokenService.GenerateTokensPairAsync(user);
                    result.Message = "Login successful. Your account is pending admin verification. You will have limited access until verified.";
                    return result;
                }
            }

            return await _tokenService.GenerateTokensPairAsync(user);
        }

        public Task<AuthResultDto> RefreshTokenAsync(string refreshToken)
        {
            return _tokenService.RefreshTokenAsync(refreshToken);
        }
        #endregion
    }
}
