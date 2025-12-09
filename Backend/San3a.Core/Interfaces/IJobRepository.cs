using San3a.Core.Entities;
using San3a.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.Interfaces
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        Task<Job?> GetJobWithDetailsAsync(string id);
        Task<IReadOnlyList<Job>> GetJobsByStatusAsync(JobStatus status);
        Task<IReadOnlyList<Job>> GetJobsByCustomerIdAsync(string customerId);
        Task<IReadOnlyList<Job>> GetJobsByServiceIdAsync(string serviceId);
    }
}
