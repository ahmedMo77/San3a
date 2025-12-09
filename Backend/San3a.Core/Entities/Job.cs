using San3a.Core.Base;
using San3a.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace San3a.Core.Entities
{
    public class Job : BaseAuditableEntity
    {
        #region Properties
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double Budget { get; set; }
        public JobStatus Status { get; set; } = JobStatus.Open;
        public PostingType PostingType { get; set; } = PostingType.Public;
        public string CustomerId { get; set; } = string.Empty;
        public string ServiceId { get; set; } = string.Empty;
        public string? AcceptedCraftsmanId { get; set; }
        public string? DirectCraftsmanId { get; set; }
        #endregion

        #region Navigation Properties
        [ForeignKey("AcceptedCraftsmanId")]
        public Craftsman? AcceptedWorker { get; set; }

        [ForeignKey("DirectCraftsmanId")]
        public Craftsman? DirectCraftsman { get; set; }

        [ForeignKey("ServiceId")]
        public Service ServiceType { get; set; } = null!;
         
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } = null!;

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public ICollection<JobRequest> DirectRequests { get; set; } = new List<JobRequest>();
        public ICollection<JobAttachment> Attachments { get; set; } = new List<JobAttachment>();
        #endregion
    }
}
