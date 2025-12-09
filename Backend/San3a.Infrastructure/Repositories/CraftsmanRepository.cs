using Microsoft.EntityFrameworkCore;
using San3a.Core.Entities;
using San3a.Core.Interfaces;
using San3a.Infrastructure.Data;

namespace San3a.Infrastructure.Repositories
{
    public class CraftsmanRepository : GenericRepository<Craftsman>, ICraftsmanRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion

        #region Constructors
        public CraftsmanRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public async Task<IReadOnlyList<Craftsman>> GetCraftsmenByServiceIdAsync(string serviceId)
        {
            return await _context.Craftsmen
                .Include(c => c.AppUser)
                .Include(c => c.Service)
                .Where(c => c.ServiceId == serviceId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Craftsman?> GetCraftsmanWithDetailsAsync(string id)
        {
            return await _context.Craftsmen
                .Include(c => c.AppUser)
                .Include(c => c.Service)
                .Include(c => c.Offers)
                .Include(c => c.AcceptedJobs)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IReadOnlyList<Craftsman>> GetVerifiedCraftsmenAsync()
        {
            return await _context.Craftsmen
                .Include(c => c.AppUser)
                .Include(c => c.Service)
                .Where(c => c.IsVerified)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Craftsman>> GetPendingVerificationCraftsmenAsync()
        {
            return await _context.Craftsmen
                .Include(c => c.AppUser)
                .Include(c => c.Service)
                .Where(c => !c.IsVerified)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion
    }
}
