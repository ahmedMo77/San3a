using Microsoft.AspNetCore.Identity;
using San3a.Core.Enums;
using System.Text.Json.Serialization;

namespace San3a.Core.Entities
{
    public class AppUser : IdentityUser
    {
        #region Properties
        public string FullName { get; set; } 
         public string Governorate {  get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Email verfication properties
        public bool IsEmailVerified { get; set; } = false;
        public string? EmailVerificationCode { get; set; }
        public DateTime? EmailVerificationCodeExpiry { get; set; }

        //reset code properties
        public string? PasswordResetCode { get; set; }
        public DateTime? PasswordResetCodeExpiry { get; set; }
        public int PasswordResetAttempts { get; set; } = 0;
        #endregion

        #region Navigation Properties
        public ICollection<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();
        #endregion
    }
}
