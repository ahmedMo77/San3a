using San3a.Core.Entities;
using San3a.Core.Enums;
using System.Linq.Expressions;

namespace San3a.Core.Specifications
{
    public class JobWithDetailsSpecification : BaseSpecification<Job>
    {
        public JobWithDetailsSpecification(string jobId) : base(j => j.Id == jobId)
        {
            AddInclude(j => j.Customer);
            AddInclude(j => j.ServiceType);
            AddInclude(j => j.AcceptedWorker);
            AddInclude(j => j.DirectCraftsman);
            AddInclude(j => j.Offers);
            AddInclude(j => j.DirectRequests);
            AddInclude(j => j.Attachments);
        }
    }

    public class JobsByStatusSpecification : BaseSpecification<Job>
    {
        public JobsByStatusSpecification(JobStatus status) : base(j => j.Status == status)
        {
            AddInclude(j => j.Customer);
            AddInclude(j => j.ServiceType);
            ApplyOrderByDescending(j => j.CreatedAt);
        }
    }

    public class JobsByCustomerSpecification : BaseSpecification<Job>
    {
        public JobsByCustomerSpecification(string customerId) : base(j => j.CustomerId == customerId)
        {
            AddInclude(j => j.ServiceType);
            AddInclude(j => j.AcceptedWorker);
            ApplyOrderByDescending(j => j.CreatedAt);
        }
    }

    public class JobsByServiceSpecification : BaseSpecification<Job>
    {
        public JobsByServiceSpecification(string serviceId) : base(j => j.ServiceId == serviceId)
        {
            AddInclude(j => j.Customer);
            ApplyOrderByDescending(j => j.CreatedAt);
        }
    }

    public class PublicJobsSpecification : BaseSpecification<Job>
    {
        public PublicJobsSpecification() : base(j => j.PostingType == PostingType.Public || j.PostingType == PostingType.Hybrid)
        {
            AddInclude(j => j.Customer);
            AddInclude(j => j.ServiceType);
            ApplyOrderByDescending(j => j.CreatedAt);
        }
    }
}
