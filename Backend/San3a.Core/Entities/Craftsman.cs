using San3a.Core.Base;
using San3a.Core.Enums;

namespace San3a.Core.Entities
{
    public class Craftsman : BaseEntity
    {
        #region Properties
        public string NationalId { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public string ServiceId { get; set; } = string.Empty;
        #endregion

        #region Navigation Properties
        public AppUser AppUser { get; set; } = null!;
        public Service Service { get; set; } = null!;
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public ICollection<Job> AcceptedJobs { get; set; } = new List<Job>();
        public ICollection<Job> DirectJobs { get; set; } = new List<Job>();
        public ICollection<JobRequest> JobRequests { get; set; } = new List<JobRequest>();
        public ICollection<CraftsmanPortfolio> Portfolios { get; set; } = new List<CraftsmanPortfolio>();
        #endregion
    }
}
