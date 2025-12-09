using Microsoft.EntityFrameworkCore;
using San3a.Core.Entities;
using San3a.Core.Interfaces;
using San3a.Infrastructure.Data;

namespace San3a.Infrastructure.Repositories
{
    public class CraftsmanPortfolioRepository : GenericRepository<CraftsmanPortfolio>, ICraftsmanPortfolioRepository
    {
        private readonly AppDbContext _context;

        public CraftsmanPortfolioRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<CraftsmanPortfolio>> GetByCraftsmanIdAsync(string craftsmanId)
        {
            return await _context.Set<CraftsmanPortfolio>()
                .Include(p => p.Images)
                .Where(p => p.CraftsmanId == craftsmanId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CraftsmanPortfolio?> GetWithImagesAsync(string portfolioId)
        {
            return await _context.Set<CraftsmanPortfolio>()
                .Include(p => p.Images)
                .Include(p => p.Craftsman)
                    .ThenInclude(c => c.AppUser)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == portfolioId);
        }

        public async Task<CraftsmanPortfolio?> GetWithImagesForUpdateAsync(string portfolioId)
        {
            return await _context.Set<CraftsmanPortfolio>()
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == portfolioId);
        }

        public async Task<IReadOnlyList<CraftsmanPortfolio>> GetAllWithImagesAsync()
        {
            return await _context.Set<CraftsmanPortfolio>()
                .Include(p => p.Images)
                .Include(p => p.Craftsman)
                    .ThenInclude(c => c.AppUser)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
