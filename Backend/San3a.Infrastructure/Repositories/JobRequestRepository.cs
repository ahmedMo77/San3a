using Microsoft.EntityFrameworkCore;
using San3a.Core.Entities;
using San3a.Core.Interfaces;
using San3a.Infrastructure.Data;

namespace San3a.Infrastructure.Repositories
{
    public class JobRequestRepository : GenericRepository<JobRequest>, IJobRequestRepository
    {
        private readonly AppDbContext _context;

        public JobRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<JobRequest>> GetByJobIdAsync(string jobId)
        {
            return await _context.Set<JobRequest>()
                .Include(jr => jr.Craftsman)
                    .ThenInclude(c => c.AppUser)
                .Where(jr => jr.JobId == jobId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<JobRequest>> GetByCraftsmanIdAsync(string craftsmanId)
        {
            return await _context.Set<JobRequest>()
                .Include(jr => jr.Job)
                    .ThenInclude(j => j.Customer)
                        .ThenInclude(c => c.AppUser)
                .Where(jr => jr.CraftsmanId == craftsmanId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
