using San3a.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.Interfaces
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task<Service?> GetServiceWithCraftsmenAsync(string id);
    }
}
