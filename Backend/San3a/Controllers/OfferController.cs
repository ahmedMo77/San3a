using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using San3a.Application.Interfaces;
using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Offer;
using San3a.WebApi.Attributes;

namespace San3a.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class OfferController : BaseApiController
    {
        #region Fields
        private readonly IOfferService _offerService;
        private readonly IJobService _jobService;
        #endregion

        #region Constructors
        public OfferController(IOfferService offerService, IJobService jobService)
        {
            _offerService = offerService;
            _jobService = jobService;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get all offers with pagination (Admin only)
        /// </summary>
        [HttpGet(Name = "GetAllOffers")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<OfferResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllOffers()
        {
            var pagination = GetPaginationParams();
            var response = await _offerService.GetPagedAsync(pagination.PageNumber, pagination.PageSize);
            return HandleResponse(response);
        }

        [HttpGet("{id}", Name = "GetOfferById")]
        [ProducesResponseType(typeof(ApiResponse<OfferResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOfferById(string id)
        {
            var response = await _offerService.GetByIdAsync(id);
            return HandleResponse(response);
        }

        [HttpGet("{id}/details", Name = "GetOfferWithDetails")]
        [ProducesResponseType(typeof(ApiResponse<OfferResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOfferWithDetails(string id)
        {
            var response = await _offerService.GetOfferWithDetailsAsync(id);
            return HandleResponse(response);
        }

        [HttpGet("by-job/{jobId}", Name = "GetOffersByJob")]
        [ProducesResponseType(typeof(ApiResponse<List<OfferResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOffersByJob(string jobId)
        {
            var jobResponse = await _jobService.GetByIdAsync(jobId);
            if (!jobResponse.Success) return NotFound(jobResponse);

            var currentUserId = GetCurrentUserId();
            if (jobResponse.Data.CustomerId != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _offerService.GetOffersByJobIdAsync(jobId);
            return HandleResponse(response);
        }

        [HttpGet("by-craftsman/{craftsmanId}", Name = "GetOffersByCraftsman")]
        [ProducesResponseType(typeof(ApiResponse<List<OfferResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOffersByCraftsman(string craftsmanId)
        {
            var currentUserId = GetCurrentUserId();
            if (craftsmanId != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _offerService.GetOffersByCraftsmanIdAsync(craftsmanId);
            return HandleResponse(response);
        }

        [HttpPost(Name = "CreateOffer")]
        [RequireVerifiedCraftsman] // ? Requires verification
        [ProducesResponseType(typeof(ApiResponse<OfferResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateOffer([FromBody] CreateOfferDto dto)
        {
            var userId = GetCurrentUserId();
            var response = await _offerService.CreateAsync(dto, userId);
            return HandleResponse(response);
        }

        [HttpPut("{id}", Name = "UpdateOffer")]
        [RequireVerifiedCraftsman] // ? Requires verification
        [ProducesResponseType(typeof(ApiResponse<OfferResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateOffer(string id, [FromBody] UpdateOfferDto dto)
        {
            var offerResponse = await _offerService.GetByIdAsync(id);
            if (!offerResponse.Success) return NotFound(offerResponse);

            var currentUserId = GetCurrentUserId();
            if (offerResponse.Data.CraftsmanId != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _offerService.UpdateAsync(id, dto);
            return HandleResponse(response);
        }

        [HttpDelete("{id}", Name = "DeleteOffer")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteOffer(string id)
        {
            var offerResponse = await _offerService.GetByIdAsync(id);
            if (!offerResponse.Success) return NotFound(offerResponse);

            var currentUserId = GetCurrentUserId();
            if (offerResponse.Data.CraftsmanId != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _offerService.DeleteAsync(id);
            return HandleResponse(response);
        }

        [HttpPatch("{id}/accept", Name = "AcceptOffer")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AcceptOffer(string id)
        {
            var offerResponse = await _offerService.GetOfferWithDetailsAsync(id);
            if (!offerResponse.Success) return NotFound(offerResponse);

            var jobResponse = await _jobService.GetByIdAsync(offerResponse.Data.JobId);
            if (!jobResponse.Success) return NotFound(jobResponse);

            var currentUserId = GetCurrentUserId();
            if (jobResponse.Data.CustomerId != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _offerService.AcceptOfferAsync(id);
            return HandleResponse(response);
        }

        [HttpPatch("{id}/reject", Name = "RejectOffer")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RejectOffer(string id)
        {
            var offerResponse = await _offerService.GetOfferWithDetailsAsync(id);
            if (!offerResponse.Success) return NotFound(offerResponse);

            var jobResponse = await _jobService.GetByIdAsync(offerResponse.Data.JobId);
            if (!jobResponse.Success) return NotFound(jobResponse);

            var currentUserId = GetCurrentUserId();
            if (jobResponse.Data.CustomerId != currentUserId && !IsAdmin())
                return Forbid();

            var response = await _offerService.RejectOfferAsync(id);
            return HandleResponse(response);
        }
        #endregion
    }
}
