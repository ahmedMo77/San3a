using San3a.Core.Entities;
using San3a.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.Interfaces
{
    public interface IOfferRepository : IGenericRepository<Offer>
    {
        Task<Offer?> GetOfferWithDetailsAsync(string id);
        Task<IReadOnlyList<Offer>> GetOffersByJobIdAsync(string jobId);
        Task<IReadOnlyList<Offer>> GetOffersByCraftsmanIdAsync(string craftsmanId);
        Task<IReadOnlyList<Offer>> GetOffersByStatusAsync(OfferStatus status);
    }
}
