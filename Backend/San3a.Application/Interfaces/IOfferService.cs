using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Offer;
using San3a.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Application.Interfaces
{
    public interface IOfferService : IBaseService<OfferResponseDto, CreateOfferDto, UpdateOfferDto>
    {
        Task<ApiResponse<List<OfferResponseDto>>> GetOffersByJobIdAsync(string jobId);
        Task<ApiResponse<List<OfferResponseDto>>> GetOffersByCraftsmanIdAsync(string craftsmanId);
        Task<ApiResponse<OfferResponseDto>> GetOfferWithDetailsAsync(string id);
        Task<ApiResponse<bool>> AcceptOfferAsync(string offerId);
        Task<ApiResponse<bool>> RejectOfferAsync(string offerId);
    }
}
