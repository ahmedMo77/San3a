using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using San3a.Application.Interfaces;
using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Craftsman;
using System.Security.Claims;

namespace San3a.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class CraftsmanController : BaseApiController
    {
        #region Fields
        private readonly ICraftsmanService _craftsmanService;
        #endregion

        #region Constructors
        public CraftsmanController(ICraftsmanService craftsmanService)
        {
            _craftsmanService = craftsmanService;
        }
        #endregion

        #region Public Methods
        [HttpGet(Name = "GetAllCraftsmen")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<CraftsmanResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCraftsmen()
        {
            var pagination = GetPaginationParams();
            var response = await _craftsmanService.GetPagedAsync(pagination.PageNumber, pagination.PageSize);
            return HandleResponse(response);
        }

        [HttpGet("{id}", Name = "GetCraftsmanById")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<CraftsmanResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCraftsmanById(string id)
        {
            var response = await _craftsmanService.GetByIdAsync(id);
            return HandleResponse(response);
        }

        [HttpGet("{id}/details", Name = "GetCraftsmanWithDetails")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<CraftsmanResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCraftsmanWithDetails(string id)
        {
            var response = await _craftsmanService.GetCraftsmanWithDetailsAsync(id);
            return HandleResponse(response);
        }

        [HttpGet("by-service/{serviceId}", Name = "GetCraftsmenByService")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<List<CraftsmanResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCraftsmenByService(string serviceId)
        {
            var response = await _craftsmanService.GetCraftsmenByServiceIdAsync(serviceId);
            return HandleResponse(response);
        }

        [HttpGet("verified", Name = "GetVerifiedCraftsmen")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<List<CraftsmanResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVerifiedCraftsmen()
        {
            var response = await _craftsmanService.GetVerifiedCraftsmenAsync();
            return HandleResponse(response);
        }

        // Admin Verification Endpoints
        [HttpGet("pending-verification", Name = "GetPendingVerificationCraftsmen")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(typeof(ApiResponse<List<CraftsmanVerificationResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPendingVerificationCraftsmen()
        {
            var response = await _craftsmanService.GetPendingVerificationCraftsmenAsync();
            return HandleResponse(response);
        }

        [HttpPost("verify", Name = "ApproveCraftsmanVerification")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ApproveCraftsmanVerification([FromBody] VerifyCraftsmanDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _craftsmanService.ApproveCraftsmanVerificationAsync(dto);
            return HandleResponse(response);
        }

        [HttpPost(Name = "CreateCraftsman")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CraftsmanResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCraftsman([FromBody] CreateCraftsmanDto dto)
        {
            var userId = GetCurrentUserId();
            var response = await _craftsmanService.CreateAsync(dto, userId);
            return HandleResponse(response);
        }

        [HttpPut("{id}", Name = "UpdateCraftsman")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CraftsmanResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCraftsman(string id, [FromBody] UpdateCraftsmanDto dto)
        {
            var currentUserId = GetCurrentUserId();
            if (id != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _craftsmanService.UpdateAsync(id, dto);
            return HandleResponse(response);
        }

        [HttpDelete("{id}", Name = "DeleteCraftsman")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCraftsman(string id)
        {
            var response = await _craftsmanService.DeleteAsync(id);
            return HandleResponse(response);
        }
        #endregion
    }
}
