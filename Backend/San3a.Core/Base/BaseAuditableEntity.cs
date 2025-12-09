using System;

namespace San3a.Core.Base
{
    public abstract class BaseAuditableEntity : BaseEntity
    {
        #region Properties
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        #endregion
    }
}
