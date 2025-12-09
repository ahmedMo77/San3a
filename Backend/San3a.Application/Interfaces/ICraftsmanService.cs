using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Craftsman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Application.Interfaces
{
    public interface ICraftsmanService : IBaseService<CraftsmanResponseDto, CreateCraftsmanDto, UpdateCraftsmanDto>
    {
        Task<ApiResponse<List<CraftsmanResponseDto>>> GetCraftsmenByServiceIdAsync(string serviceId);
        Task<ApiResponse<CraftsmanResponseDto>> GetCraftsmanWithDetailsAsync(string id);
        Task<ApiResponse<List<CraftsmanResponseDto>>> GetVerifiedCraftsmenAsync();
        
        // Admin verification endpoints
        Task<ApiResponse<List<CraftsmanVerificationResponseDto>>> GetPendingVerificationCraftsmenAsync();
        Task<ApiResponse<bool>> ApproveCraftsmanVerificationAsync(VerifyCraftsmanDto dto);
    }
}
