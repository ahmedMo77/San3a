using Microsoft.EntityFrameworkCore;
using San3a.Core.Entities;
using San3a.Core.Enums;
using San3a.Core.Interfaces;
using San3a.Infrastructure.Data;

namespace San3a.Infrastructure.Repositories
{
    public class OfferRepository : GenericRepository<Offer>, IOfferRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion

        #region Constructors
        public OfferRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public async Task<Offer?> GetOfferWithDetailsAsync(string id)
        {
            return await _context.Offers
                .Include(o => o.Job)
                    .ThenInclude(j => j.Customer)
                        .ThenInclude(c => c.AppUser)
                .Include(o => o.Worker)
                    .ThenInclude(w => w.AppUser)
                .Include(o => o.Worker)
                    .ThenInclude(w => w.Service)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IReadOnlyList<Offer>> GetOffersByJobIdAsync(string jobId)
        {
            return await _context.Offers
                .Include(o => o.Worker)
                    .ThenInclude(w => w.AppUser)
                .Include(o => o.Worker)
                    .ThenInclude(w => w.Service)
                .Where(o => o.JobId == jobId)
                .OrderByDescending(o => o.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Offer>> GetOffersByCraftsmanIdAsync(string craftsmanId)
        {
            return await _context.Offers
                .Include(o => o.Job)
                    .ThenInclude(j => j.Customer)
                        .ThenInclude(c => c.AppUser)
                .Include(o => o.Job)
                    .ThenInclude(j => j.ServiceType)
                .Where(o => o.CraftsmanId == craftsmanId)
                .OrderByDescending(o => o.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Offer>> GetOffersByStatusAsync(OfferStatus status)
        {
            return await _context.Offers
                .Include(o => o.Job)
                .Include(o => o.Worker)
                    .ThenInclude(w => w.AppUser)
                .Where(o => o.Status == status)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion
    }
}
