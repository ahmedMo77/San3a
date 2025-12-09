using Microsoft.EntityFrameworkCore;
using San3a.Core.Entities;
using San3a.Core.Interfaces;
using San3a.Infrastructure.Data;

namespace San3a.Infrastructure.Repositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion

        #region Constructors
        public ServiceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public async Task<Service?> GetServiceWithCraftsmenAsync(string id)
        {
            return await _context.Services
                .Include(s => s.Craftsmen)
                    .ThenInclude(c => c.AppUser)
                .Include(s => s.Jobs)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        #endregion
    }
}
