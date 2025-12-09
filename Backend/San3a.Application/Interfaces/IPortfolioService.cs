using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Portfolio;
using Microsoft.AspNetCore.Http;

namespace San3a.Application.Interfaces
{
    public interface IPortfolioService
    {
        Task<ApiResponse<PortfolioResponseDto>> CreatePortfolioAsync(CreatePortfolioDto dto, IEnumerable<IFormFile> images, string craftsmanId);
        Task<ApiResponse<PortfolioResponseDto>> UpdatePortfolioAsync(string portfolioId, UpdatePortfolioDto dto, string craftsmanId);
        Task<ApiResponse<bool>> DeletePortfolioAsync(string portfolioId, string craftsmanId);
        Task<ApiResponse<PortfolioResponseDto>> GetPortfolioByIdAsync(string portfolioId);
        Task<ApiResponse<List<PortfolioResponseDto>>> GetPortfoliosByCraftsmanIdAsync(string craftsmanId);
        Task<ApiResponse<List<PortfolioResponseDto>>> GetAllPortfoliosAsync();
        Task<ApiResponse<bool>> AddImagesToPortfolioAsync(string portfolioId, IEnumerable<IFormFile> images, string craftsmanId);
        Task<ApiResponse<bool>> RemoveImageFromPortfolioAsync(string portfolioId, string imageId, string craftsmanId);
        Task<ApiResponse<PortfolioRequestResponseDto>> CreatePortfolioRequestAsync(CreatePortfolioRequestDto dto, string customerId);
        Task<ApiResponse<bool>> RespondToPortfolioRequestAsync(string requestId, string craftsmanId, bool accept);
        Task<ApiResponse<List<PortfolioRequestResponseDto>>> GetCustomerPortfolioRequestsAsync(string customerId);
        Task<ApiResponse<List<PortfolioRequestResponseDto>>> GetCraftsmanPortfolioRequestsAsync(string craftsmanId);
    }
}
