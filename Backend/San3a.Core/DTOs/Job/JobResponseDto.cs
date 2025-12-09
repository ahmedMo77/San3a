using San3a.Core.DTOs.Base;
using San3a.Core.Enums;

namespace San3a.Core.DTOs.Job
{
    public class JobResponseDto : BaseAuditableResponseDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double Budget { get; set; }
        public JobStatus Status { get; set; }
        public PostingType PostingType { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string ServiceId { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string? AcceptedCraftsmanId { get; set; }
        public string? AcceptedCraftsmanName { get; set; }
        public string? DirectCraftsmanId { get; set; }
        public string? DirectCraftsmanName { get; set; }
        public int OffersCount { get; set; }
        public int DirectRequestsCount { get; set; }
    }
}
