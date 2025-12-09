using San3a.Core.Entities;
using San3a.Core.Interfaces;

namespace San3a.Core.Interfaces
{
    public interface ICraftsmanPortfolioRepository : IGenericRepository<CraftsmanPortfolio>
    {
        Task<IReadOnlyList<CraftsmanPortfolio>> GetByCraftsmanIdAsync(string craftsmanId);
        Task<CraftsmanPortfolio?> GetWithImagesAsync(string portfolioId);
        Task<CraftsmanPortfolio?> GetWithImagesForUpdateAsync(string portfolioId);
        Task<IReadOnlyList<CraftsmanPortfolio>> GetAllWithImagesAsync();
    }
}
