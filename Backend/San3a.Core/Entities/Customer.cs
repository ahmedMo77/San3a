using San3a.Core.Base;

namespace San3a.Core.Entities
{
    public class Customer : BaseEntity
    {
        #region Properties
        public string? NationalId { get; set; }
        #endregion

        #region Navigation Properties
        public AppUser AppUser { get; set; } = null!;
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
        public ICollection<PortfolioRequest> PortfolioRequests { get; set; } = new List<PortfolioRequest>();
        #endregion
    }
}

