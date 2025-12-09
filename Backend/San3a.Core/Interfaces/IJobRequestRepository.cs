using San3a.Core.Entities;
using San3a.Core.Interfaces;

namespace San3a.Core.Interfaces
{
    public interface IJobRequestRepository : IGenericRepository<JobRequest>
    {
        Task<IReadOnlyList<JobRequest>> GetByJobIdAsync(string jobId);
        Task<IReadOnlyList<JobRequest>> GetByCraftsmanIdAsync(string craftsmanId);
    }
}
