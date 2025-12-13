using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using San3a.Application.Interfaces;
using San3a.Core.Entities;
using San3a.Infrastructure.Data;
using San3a.Application.Helpers;
using San3a.Core.DTOs.auth;
namespace San3a.Application.Services
{
    public class AuthService : IAuthService
    {
        #region Fields
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public AuthService(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            IEmailService emailService,
            AppDbContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods

        #region Registration Methods
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

           
            try
            {
                await SendVerificationEmailAsync(user);
                return new AuthResultDto
                {
                    Success = true,
                    Message = "Admin account created successfully. Please check your email for the verification Code."
                };
            }
            catch
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = "Failed to send verification email."
                };
            }
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
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.NationalId == dto.NationalId);
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

          
            try
            {
                await SendVerificationEmailAsync(user);
                return new AuthResultDto
                {
                    Success = true,
                    Message = "Registration successful. Please check your email for the verification code."
                };
            }
            catch
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = "Failed to send verification email."
                };
            }
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
            var existingCraftsman = await _context.Craftsmen
                .FirstOrDefaultAsync(c => c.NationalId == dto.NationalId);
            if (existingCraftsman != null)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = "National ID already exists"
                };
            }

            
            var serviceExists = await _context.Services
                .AnyAsync(s => s.Id == dto.ServiceId);
            if (!serviceExists)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = "Invalid Service ID. Please select a valid service."
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // If save fails, delete the created user
                await _userManager.DeleteAsync(user);

                return new AuthResultDto
                {
                    Success = false,
                    Message = "Failed to complete registration. Please try again."
                };
            }

            try
            {
                await SendVerificationEmailAsync(user);
                return new AuthResultDto
                {
                    Success = true,
                    Message = "Registration successful. Please check your email for the verification link. Your account is also pending admin verification."
                };
            }
            catch
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = "Failed to send verification email."
                };
            }
        }
        #endregion

        #region Login Methods
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
            if (!user.IsEmailVerified)
            {
                await SendVerificationEmailAsync(user);
                return new AuthResultDto
                {
                    Success = false,
                    Message = "Email verification required. A new verification link has been sent to your email."
                };
            }
            // Check if user is a craftsman and if verified
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Craftsman"))
            {
                var craftsman = await _context.Craftsmen.FindAsync(user.Id);
                if (craftsman != null && !craftsman.IsVerified)
                {
                    // Generate tokens with refresh token on login
                    var result = await _tokenService.GenerateTokensPairAsync(user);
                    result.Message = "Login successful. Your account is pending admin verification. You will have limited access until verified.";
                    return result;
                }
            }

         
            return await _tokenService.GenerateTokensPairAsync(user);
        }
        #endregion

        #region Token Methods
        public Task<AuthResultDto> RefreshTokenAsync(string refreshToken)
        {
            return _tokenService.RefreshTokenAsync(refreshToken);
        }
        #endregion

        #region Email Verification Methods
        public async Task<(bool Success, string Message)> VerifyEmailAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return (false, "Verification token is required");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.EmailVerificationCode == token);

            if (user == null)
                return (false, "Invalid or expired verification code");

            if (user.IsEmailVerified)
                return (false, "Email already verified");

            if (user.EmailVerificationCodeExpiry.HasValue &&
                user.EmailVerificationCodeExpiry.Value < DateTime.UtcNow)
                return (false, "Verification code has expired");

            user.IsEmailVerified = true;
            user.EmailVerificationCode = null;
            user.EmailVerificationCodeExpiry = null;

            await _userManager.UpdateAsync(user);

            return (true, "Email verified successfully");
        }

        public async Task<(bool Success, string Message)> ResendVerificationCodeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return (false, "User not found");

            if (user.IsEmailVerified)
                return (false, "Email is already verified");

            await SendVerificationEmailAsync(user);
            return (true, "Verification link has been sent to your email");
        }
        #endregion

        #region Password Reset Methods
        public async Task<(bool Success, string Message)> RequestPasswordResetAsync(string email)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    await transaction.RollbackAsync();
                    return (false, "User not found");
                }

                var resetCode = CodeGenerator.GenerateCode();
                user.PasswordResetCode = resetCode;
                user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(5);
                user.PasswordResetAttempts = 0;

                await _userManager.UpdateAsync(user);
                await transaction.CommitAsync();

                await _emailService.SendPasswordResetCodeAsync(email, resetCode);
                return (true, "Password reset code has been sent to your email");
            }
            catch
            {
                await transaction.RollbackAsync();
                return (false, "An error occurred while processing password reset request");
            }
        }

        public async Task<(bool Success, string Message, string? ResetToken)> VerifyResetCodeAsync(string email, string code)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
                return (false, "Email and code are required", null);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || string.IsNullOrEmpty(user.PasswordResetCode))
                {
                    await transaction.RollbackAsync();
                    return (false, "Invalid email or reset code", null);
                }

                if (user.PasswordResetCodeExpiry.HasValue &&
                    user.PasswordResetCodeExpiry.Value < DateTime.UtcNow)
                {
                    await transaction.RollbackAsync();
                    return (false, "Reset code has expired. Please request a new one.", null);
                }

                if (user.PasswordResetAttempts >= 5)
                {
                    await transaction.RollbackAsync();
                    return (false, "Too many failed attempts. Please request a new reset code.", null);
                }

                if (user.PasswordResetCode != code)
                {
                    user.PasswordResetAttempts++;
                    await _userManager.UpdateAsync(user);
                    await transaction.CommitAsync();

                    var remainingAttempts = 5 - user.PasswordResetAttempts;
                    return (false, $"Invalid reset code. {remainingAttempts} attempts remaining.", null);
                }

                var resetToken = CodeGenerator.GenerateToken();
                user.PasswordResetCode = resetToken;
                user.PasswordResetAttempts = 0;

                await _userManager.UpdateAsync(user);
                await transaction.CommitAsync();

                return (true, "Reset code verified successfully", resetToken);
            }
            catch
            {
                await transaction.RollbackAsync();
                return (false, "An error occurred while verifying reset code", null);
            }
        }

        public async Task<(bool Success, string Message)> ResetPasswordAsync(string token, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(token))
                return (false, "Reset token is required");

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
                return (false, "Password and confirmation are required");

            if (newPassword != confirmPassword)
                return (false, "Passwords do not match");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PasswordResetCode == token);

                if (user == null)
                {
                    await transaction.RollbackAsync();
                    return (false, "Invalid or expired reset token");
                }

                if (user.PasswordResetCodeExpiry.HasValue &&
                    user.PasswordResetCodeExpiry.Value < DateTime.UtcNow)
                {
                    
                    await transaction.RollbackAsync();
                    return (false, "Reset token has expired");
                }

                if (user.PasswordResetAttempts >= 3)
                {
                    await transaction.RollbackAsync();
                    return (false, "Too many reset attempts. Please request a new reset code.");
                }

          
                await _userManager.RemovePasswordAsync(user);
                var result = await _userManager.AddPasswordAsync(user, newPassword);

                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return (false, string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                user.PasswordResetCode = null;
                user.PasswordResetCodeExpiry = null;
                user.PasswordResetAttempts = 0;

                await _userManager.UpdateAsync(user);
                await transaction.CommitAsync();

                return (true, "Password reset successfully");
            }
            catch
            {
                await transaction.RollbackAsync();
                return (false, "An error occurred while resetting password");
            }
        }
        public async Task<(bool Success, string Message)> ResendPasswordResetCodeAsync(string email)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    await transaction.RollbackAsync();
                    return (false, "User not found");
                }

                if (string.IsNullOrEmpty(user.PasswordResetCode))
                {
                    await transaction.RollbackAsync();
                    return (false, "No password reset request found. Please request a new one.");
                }

                
                var resetCode = CodeGenerator.GenerateCode();
                user.PasswordResetCode = resetCode;
                user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(5);
                user.PasswordResetAttempts = 0;

                await _userManager.UpdateAsync(user);
                await transaction.CommitAsync();

                await _emailService.SendPasswordResetCodeAsync(email, resetCode);
                return (true, "A new password reset code has been sent to your email");
            }
            catch
            {
                await transaction.RollbackAsync();
                return (false, "An error occurred while resending the reset code");
            }
        }
        #endregion

        #endregion

        #region Private Helper Methods
        private async Task SendVerificationEmailAsync(AppUser user)
        {
            var verificationToken = CodeGenerator.GenerateCode();
            user.EmailVerificationCode = verificationToken;
            user.EmailVerificationCodeExpiry = DateTime.UtcNow.AddMinutes(5);

            await _userManager.UpdateAsync(user);
            await _emailService.SendVerificationCodeAsync(user.Email!, verificationToken);
        }

      

       
        #endregion
    }
}