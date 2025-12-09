using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using San3a.Application.Interfaces;
using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Service;

namespace San3a.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class ServiceTypeController : BaseApiController
    {
        #region Fields
        private readonly IServiceTypeService _serviceTypeService;
        #endregion

        #region Constructors
        public ServiceTypeController(IServiceTypeService serviceTypeService)
        {
            _serviceTypeService = serviceTypeService;
        }
        #endregion

        #region Public Methods
        [HttpGet(Name = "GetAllServices")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<ServiceResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllServices()
        {
            var pagination = GetPaginationParams();
            var response = await _serviceTypeService.GetPagedAsync(pagination.PageNumber, pagination.PageSize);
            return HandleResponse(response);
        }

        [HttpGet("{id}", Name = "GetServiceById")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<ServiceResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServiceById(string id)
        {
            var response = await _serviceTypeService.GetByIdAsync(id);
            return HandleResponse(response);
        }

        [HttpGet("{id}/with-craftsmen", Name = "GetServiceWithCraftsmen")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<ServiceResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServiceWithCraftsmen(string id)
        {
            var response = await _serviceTypeService.GetServiceWithCraftsmenAsync(id);
            return HandleResponse(response);
        }

        [HttpPost(Name = "CreateService")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(typeof(ApiResponse<ServiceResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceDto dto)
        {
            var userId = GetCurrentUserId();
            var response = await _serviceTypeService.CreateAsync(dto, userId);
            return HandleResponse(response);
        }

        [HttpPut("{id}", Name = "UpdateService")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(typeof(ApiResponse<ServiceResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateService(string id, [FromBody] UpdateServiceDto dto)
        {
            var response = await _serviceTypeService.UpdateAsync(id, dto);
            return HandleResponse(response);
        }

        [HttpDelete("{id}", Name = "DeleteService")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteService(string id)
        {
            var response = await _serviceTypeService.DeleteAsync(id);
            return HandleResponse(response);
        }
        #endregion
    }
}
