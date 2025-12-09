using San3a.Core.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Application.Interfaces
{
    public interface IBaseService<TResponseDto, TCreateDto, TUpdateDto>
        where TResponseDto : class
        where TCreateDto : class
        where TUpdateDto : class
    {
        Task<ApiResponse<TResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<List<TResponseDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<TResponseDto>>> GetPagedAsync(int pageNumber, int pageSize);
        Task<ApiResponse<PagedResponse<TResponseDto>>> GetPagedAsync(PaginationParams pagination);
        Task<ApiResponse<TResponseDto>> CreateAsync(TCreateDto dto, string userId);
        Task<ApiResponse<TResponseDto>> UpdateAsync(string id, TUpdateDto dto);
        Task<ApiResponse<bool>> DeleteAsync(string id);
    }
}
