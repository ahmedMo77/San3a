using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Application.Interfaces
{
    public interface IServiceTypeService : IBaseService<ServiceResponseDto, CreateServiceDto, UpdateServiceDto>
    {
        Task<ApiResponse<ServiceResponseDto>> GetServiceWithCraftsmenAsync(string id);
    }
}
