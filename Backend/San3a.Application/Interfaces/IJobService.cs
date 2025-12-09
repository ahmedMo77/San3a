using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Job;
using San3a.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Application.Interfaces
{
    public interface IJobService : IBaseService<JobResponseDto, CreateJobDto, UpdateJobDto>
    {
        Task<ApiResponse<JobResponseDto>> CreateJobAsync(CreateJobWithDirectRequestDto dto, string customerId);
        Task<ApiResponse<bool>> RespondToJobRequestAsync(string jobRequestId, string craftsmanId, bool accept);
        Task<ApiResponse<List<JobResponseDto>>> GetJobsByStatusAsync(JobStatus status);
        Task<ApiResponse<List<JobResponseDto>>> GetJobsByCustomerIdAsync(string customerId);
        Task<ApiResponse<List<JobResponseDto>>> GetJobsByServiceIdAsync(string serviceId);
        Task<ApiResponse<JobResponseDto>> GetJobWithDetailsAsync(string id);
        Task<ApiResponse<bool>> AssignJobToCraftsmanAsync(string jobId, string craftsmanId);
        Task<ApiResponse<bool>> UpdateJobStatusAsync(string jobId, JobStatus status);
    }
}
