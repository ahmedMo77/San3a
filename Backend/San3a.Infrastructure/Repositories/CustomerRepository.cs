using Microsoft.EntityFrameworkCore;
using San3a.Core.Entities;
using San3a.Core.Interfaces;
using San3a.Infrastructure.Data;

namespace San3a.Infrastructure.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion

        #region Constructors
        public CustomerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public async Task<Customer?> GetCustomerWithDetailsAsync(string id)
        {
            return await _context.Customers
                .Include(c => c.AppUser)
                .Include(c => c.Jobs)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IReadOnlyList<Job>> GetCustomerJobsAsync(string customerId)
        {
            return await _context.JobPosts
                .Include(j => j.ServiceType)
                .Include(j => j.AcceptedWorker)
                .Include(j => j.Offers)
                .Where(j => j.CustomerId == customerId)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion
    }
}
