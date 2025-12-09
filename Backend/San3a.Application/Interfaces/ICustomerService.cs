using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Customer;
using San3a.Core.DTOs.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Application.Interfaces
{
    public interface ICustomerService : IBaseService<CustomerResponseDto, object, object>
    {
        Task<ApiResponse<CustomerResponseDto>> GetCustomerWithDetailsAsync(string id);
        Task<ApiResponse<List<JobResponseDto>>> GetCustomerJobsAsync(string customerId);
        Task<ApiResponse<CustomerResponseDto>> CreateCustomerAsync(string userId);
    }
}
