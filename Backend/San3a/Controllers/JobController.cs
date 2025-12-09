using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using San3a.Application.Interfaces;
using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Job;
using San3a.Core.Enums;

namespace San3a.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class JobController : BaseApiController
    {
        #region Fields
        private readonly IJobService _jobService;
        #endregion

        #region Constructors
        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }
        #endregion

        #region Public Methods
        [HttpGet(Name = "GetAllJobs")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllJobs()
        {
            var pagination = GetPaginationParams();
            var response = await _jobService.GetPagedAsync(pagination.PageNumber, pagination.PageSize);
            return HandleResponse(response);
        }

        [HttpGet("{id}", Name = "GetJobById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetJobById(string id)
        {
            var response = await _jobService.GetJobWithDetailsAsync(id);
            return HandleResponse(response);
        }

        [HttpGet("by-status/{status}", Name = "GetJobsByStatus")]
        [AllowAnonymous]
        public async Task<IActionResult> GetJobsByStatus(string status)
        {
            if (!Enum.TryParse<JobStatus>(status, true, out var jobStatus))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Invalid job status. Valid values are: {string.Join(", ", Enum.GetNames(typeof(JobStatus)))}"
                });
            }

            var response = await _jobService.GetJobsByStatusAsync(jobStatus);
            return HandleResponse(response);
        }

        [HttpGet("by-customer/{customerId}", Name = "GetJobsByCustomer")]
        public async Task<IActionResult> GetJobsByCustomer(string customerId)
        {
            var currentUserId = GetCurrentUserId();
            if (customerId != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _jobService.GetJobsByCustomerIdAsync(customerId);
            return HandleResponse(response);
        }

        [HttpGet("by-service/{serviceId}", Name = "GetJobsByService")]
        [AllowAnonymous]
        public async Task<IActionResult> GetJobsByService(string serviceId)
        {
            var response = await _jobService.GetJobsByServiceIdAsync(serviceId);
            return HandleResponse(response);
        }

        [HttpPost(Name = "CreateJob")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobWithDirectRequestDto dto)
        {
            var userId = GetCurrentUserId();
            var response = await _jobService.CreateJobAsync(dto, userId);
            return HandleResponse(response);
        }

        [HttpPut("{id}", Name = "UpdateJob")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateJob(string id, [FromBody] UpdateJobDto dto)
        {
            var jobResponse = await _jobService.GetByIdAsync(id);
            if (!jobResponse.Success)
                return NotFound(jobResponse);

            var currentUserId = GetCurrentUserId();
            if (jobResponse.Data.CustomerId != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _jobService.UpdateAsync(id, dto);
            return HandleResponse(response);
        }

        [HttpDelete("{id}", Name = "DeleteJob")]
        [Authorize(Roles = "Customer,Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteJob(string id)
        {
            var jobResponse = await _jobService.GetByIdAsync(id);
            if (!jobResponse.Success)
                return NotFound(jobResponse);

            var currentUserId = GetCurrentUserId();
            if (jobResponse.Data.CustomerId != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _jobService.DeleteAsync(id);
            return HandleResponse(response);
        }

        [HttpPost("{jobId}/assign/{craftsmanId}", Name = "AssignJobToCraftsman")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AssignJobToCraftsman(string jobId, string craftsmanId)
        {
            var jobResponse = await _jobService.GetByIdAsync(jobId);
            if (!jobResponse.Success)
                return NotFound(jobResponse);

            var currentUserId = GetCurrentUserId();
            if (jobResponse.Data.CustomerId != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _jobService.AssignJobToCraftsmanAsync(jobId, craftsmanId);
            return HandleResponse(response);
        }

        [HttpPost("job-request/{jobRequestId}/respond", Name = "RespondToJobRequest")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> RespondToJobRequest(string jobRequestId, [FromBody] bool accept)
        {
            var currentUserId = GetCurrentUserId();
            var response = await _jobService.RespondToJobRequestAsync(jobRequestId, currentUserId, accept);
            return HandleResponse(response);
        }

        [HttpPatch("{id}/status", Name = "UpdateJobStatus")]
        public async Task<IActionResult> UpdateJobStatus(string id, [FromBody] JobStatus status)
        {
            var jobResponse = await _jobService.GetByIdAsync(id);
            if (!jobResponse.Success)
                return NotFound(jobResponse);

            var currentUserId = GetCurrentUserId();
            if (jobResponse.Data.CustomerId != currentUserId && 
                jobResponse.Data.AcceptedCraftsmanId != currentUserId && 
                !IsAdmin())
                return Forbid();

            var response = await _jobService.UpdateJobStatusAsync(id, status);
            return HandleResponse(response);
        }
        #endregion
    }
}
