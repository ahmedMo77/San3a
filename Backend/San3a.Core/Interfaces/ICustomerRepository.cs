using San3a.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetCustomerWithDetailsAsync(string id);
        Task<IReadOnlyList<Job>> GetCustomerJobsAsync(string customerId);
    }
}
