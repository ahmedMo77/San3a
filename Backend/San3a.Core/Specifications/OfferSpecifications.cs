using San3a.Core.Entities;
using San3a.Core.Enums;

namespace San3a.Core.Specifications
{
    public class OffersByJobSpecification : BaseSpecification<Offer>
    {
        public OffersByJobSpecification(string jobId) : base(o => o.JobId == jobId)
        {
            AddInclude(o => o.Worker);
            AddInclude("Worker.AppUser");
            ApplyOrderByDescending(o => o.CreatedAt);
        }
    }

    public class OffersByCraftsmanSpecification : BaseSpecification<Offer>
    {
        public OffersByCraftsmanSpecification(string craftsmanId) : base(o => o.CraftsmanId == craftsmanId)
        {
            AddInclude(o => o.Job);
            AddInclude("Job.Customer");
            AddInclude("Job.Customer.AppUser");
            ApplyOrderByDescending(o => o.CreatedAt);
        }
    }

    public class PendingOffersByJobSpecification : BaseSpecification<Offer>
    {
        public PendingOffersByJobSpecification(string jobId) 
            : base(o => o.JobId == jobId && o.Status == OfferStatus.Pending)
        {
            AddInclude(o => o.Worker);
            AddInclude("Worker.AppUser");
            ApplyOrderByDescending(o => o.CreatedAt);
        }
    }
}
