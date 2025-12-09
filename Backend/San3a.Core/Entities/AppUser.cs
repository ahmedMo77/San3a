using Microsoft.AspNetCore.Identity;
using San3a.Core.Enums;
using System.Text.Json.Serialization;

namespace San3a.Core.Entities
{
    public class AppUser : IdentityUser
    {
        #region Properties
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        #endregion

        #region Navigation Properties
        public ICollection<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();
        #endregion
    }
}
