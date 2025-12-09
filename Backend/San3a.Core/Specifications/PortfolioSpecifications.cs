using San3a.Core.Entities;
using San3a.Core.Enums;

namespace San3a.Core.Specifications
{
    public class PortfolioWithImagesSpecification : BaseSpecification<CraftsmanPortfolio>
    {
        public PortfolioWithImagesSpecification(string portfolioId) : base(p => p.Id == portfolioId)
        {
            AddInclude(p => p.Images);
            AddInclude(p => p.Craftsman);
        }
    }

    public class PortfoliosByCraftsmanSpecification : BaseSpecification<CraftsmanPortfolio>
    {
        public PortfoliosByCraftsmanSpecification(string craftsmanId) : base(p => p.CraftsmanId == craftsmanId)
        {
            AddInclude(p => p.Images);
            ApplyOrderByDescending(p => p.CreatedAt);
        }
    }

    public class VerifiedCraftsmanPortfoliosSpecification : BaseSpecification<CraftsmanPortfolio>
    {
        public VerifiedCraftsmanPortfoliosSpecification() : base(p => p.Craftsman.IsVerified)
        {
            AddInclude(p => p.Images);
            AddInclude(p => p.Craftsman);
            ApplyOrderByDescending(p => p.CreatedAt);
        }
    }

    public class PortfolioRequestsByCustomerSpecification : BaseSpecification<PortfolioRequest>
    {
        public PortfolioRequestsByCustomerSpecification(string customerId) : base(pr => pr.CustomerId == customerId)
        {
            AddInclude(pr => pr.Portfolio);
            AddInclude("Portfolio.Images");
            ApplyOrderByDescending(pr => pr.CreatedAt);
        }
    }

    public class PortfolioRequestsByPortfolioSpecification : BaseSpecification<PortfolioRequest>
    {
        public PortfolioRequestsByPortfolioSpecification(string portfolioId) : base(pr => pr.PortfolioId == portfolioId)
        {
            AddInclude(pr => pr.Customer);
            AddInclude("Customer.AppUser");
            ApplyOrderByDescending(pr => pr.CreatedAt);
        }
    }

    public class PendingPortfolioRequestsSpecification : BaseSpecification<PortfolioRequest>
    {
        public PendingPortfolioRequestsSpecification(string craftsmanId) 
            : base(pr => pr.Portfolio.CraftsmanId == craftsmanId && pr.Status == OfferStatus.Pending)
        {
            AddInclude(pr => pr.Portfolio);
            AddInclude(pr => pr.Customer);
            AddInclude("Customer.AppUser");
            ApplyOrderByDescending(pr => pr.CreatedAt);
        }
    }
}
