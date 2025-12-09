using San3a.Core.Base;

namespace San3a.Core.Entities
{
    public class Admin : BaseEntity
    {
        #region Properties
        public bool IsSuperAdmin { get; set; }
        #endregion

        #region Navigation Properties
        public AppUser AppUser { get; set; }
        #endregion
    }
}
