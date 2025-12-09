using San3a.Core.Entities;
using San3a.Core.Interfaces;

namespace San3a.Core.Interfaces
{
    public interface IPortfolioRequestRepository : IGenericRepository<PortfolioRequest>
    {
        Task<IReadOnlyList<PortfolioRequest>> GetByCustomerIdAsync(string customerId);
        Task<IReadOnlyList<PortfolioRequest>> GetByPortfolioIdAsync(string portfolioId);
    }
}
