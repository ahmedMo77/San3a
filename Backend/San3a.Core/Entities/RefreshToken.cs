using Microsoft.EntityFrameworkCore;

namespace San3a.Core.Entities
{
    [Owned]
    public class RefreshToken
    {
        #region Properties
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public bool IsActive => RevokedAt == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        #endregion
    }
}
