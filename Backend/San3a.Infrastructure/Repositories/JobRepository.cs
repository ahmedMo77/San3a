using Microsoft.EntityFrameworkCore;
using San3a.Core.Entities;
using San3a.Core.Enums;
using San3a.Core.Interfaces;
using San3a.Infrastructure.Data;

namespace San3a.Infrastructure.Repositories
{
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion

        #region Constructors
        public JobRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public async Task<Job?> GetJobWithDetailsAsync(string id)
        {
            return await _context.JobPosts
                .Include(j => j.Customer)
                    .ThenInclude(c => c.AppUser)
                .Include(j => j.ServiceType)
                .Include(j => j.AcceptedWorker)
                    .ThenInclude(w => w!.AppUser)
                .Include(j => j.DirectCraftsman)
                    .ThenInclude(dc => dc!.AppUser)
                .Include(j => j.Offers)
                    .ThenInclude(o => o.Worker)
                        .ThenInclude(w => w.AppUser)
                .Include(j => j.DirectRequests)
                    .ThenInclude(dr => dr.Craftsman)
                        .ThenInclude(c => c.AppUser)
                .Include(j => j.Attachments)
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<IReadOnlyList<Job>> GetJobsByStatusAsync(JobStatus status)
        {
            return await _context.JobPosts
                .Include(j => j.Customer)
                    .ThenInclude(c => c.AppUser)
                .Include(j => j.ServiceType)
                .Where(j => j.Status == status)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Job>> GetJobsByCustomerIdAsync(string customerId)
        {
            return await _context.JobPosts
                .Include(j => j.ServiceType)
                .Include(j => j.AcceptedWorker)
                    .ThenInclude(w => w!.AppUser)
                .Where(j => j.CustomerId == customerId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Job>> GetJobsByServiceIdAsync(string serviceId)
        {
            return await _context.JobPosts
                .Include(j => j.Customer)
                    .ThenInclude(c => c.AppUser)
                .Include(j => j.ServiceType)
                .Where(j => j.ServiceId == serviceId)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion
    }
}
