using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using San3a.Application.Interfaces;
using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Customer;
using San3a.Core.DTOs.Job;

namespace San3a.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class CustomerController : BaseApiController
    {
        #region Fields
        private readonly ICustomerService _customerService;
        #endregion

        #region Constructors
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        #endregion

        #region Public Methods
        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<CustomerResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCustomers()
        {
            var pagination = GetPaginationParams();
            var response = await _customerService.GetPagedAsync(pagination.PageNumber, pagination.PageSize);
            return HandleResponse(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CustomerResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerById(string id)
        {
            var currentUserId = GetCurrentUserId();
            if (id != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _customerService.GetByIdAsync(id);
            return HandleResponse(response);
        }

        [HttpGet("{id}/details")]
        [ProducesResponseType(typeof(ApiResponse<CustomerResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerWithDetails(string id)
        {
            var currentUserId = GetCurrentUserId();
            if (id != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _customerService.GetCustomerWithDetailsAsync(id);
            return HandleResponse(response);
        }

        [HttpGet("{id}/jobs")]
        [ProducesResponseType(typeof(ApiResponse<List<JobResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerJobs(string id)
        {
            var currentUserId = GetCurrentUserId();
            if (id != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _customerService.GetCustomerJobsAsync(id);
            return HandleResponse(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CustomerResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCustomer()
        {
            var userId = GetCurrentUserId();
            var response = await _customerService.CreateCustomerAsync(userId);
            return HandleResponse(response);
        }
        #endregion
    }
}
