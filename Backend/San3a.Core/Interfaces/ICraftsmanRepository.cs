using San3a.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.Interfaces
{
    public interface ICraftsmanRepository : IGenericRepository<Craftsman>
    {
        Task<IReadOnlyList<Craftsman>> GetCraftsmenByServiceIdAsync(string serviceId);
        Task<Craftsman?> GetCraftsmanWithDetailsAsync(string id);
        Task<IReadOnlyList<Craftsman>> GetVerifiedCraftsmenAsync();
        Task<IReadOnlyList<Craftsman>> GetPendingVerificationCraftsmenAsync();
    }
}
