using Microsoft.EntityFrameworkCore;
using San3a.Core.Entities;
using San3a.Core.Interfaces;
using San3a.Infrastructure.Data;

namespace San3a.Infrastructure.Repositories
{
    public class PortfolioRequestRepository : GenericRepository<PortfolioRequest>, IPortfolioRequestRepository
    {
        private readonly AppDbContext _context;

        public PortfolioRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<PortfolioRequest>> GetByCustomerIdAsync(string customerId)
        {
            return await _context.Set<PortfolioRequest>()
                .Include(pr => pr.Portfolio)
                    .ThenInclude(p => p.Images)
                .Where(pr => pr.CustomerId == customerId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<PortfolioRequest>> GetByPortfolioIdAsync(string portfolioId)
        {
            return await _context.Set<PortfolioRequest>()
                .Include(pr => pr.Customer)
                    .ThenInclude(c => c.AppUser)
                .Where(pr => pr.PortfolioId == portfolioId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
