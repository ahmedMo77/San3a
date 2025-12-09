using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using San3a.Application.Interfaces;
using San3a.Core.DTOs.Portfolio;
using San3a.WebApi.Attributes;

namespace San3a.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PortfolioController : BaseApiController
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpGet(Name = "GetAllPortfolios")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPortfolios()
        {
            var response = await _portfolioService.GetAllPortfoliosAsync();
            return HandleResponse(response);
        }

        [HttpGet("{id}", Name = "GetPortfolioById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPortfolioById(string id)
        {
            var response = await _portfolioService.GetPortfolioByIdAsync(id);
            return HandleResponse(response);
        }

        [HttpGet("craftsman/{craftsmanId}", Name = "GetPortfoliosByCraftsman")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPortfoliosByCraftsman(string craftsmanId)
        {
            var response = await _portfolioService.GetPortfoliosByCraftsmanIdAsync(craftsmanId);
            return HandleResponse(response);
        }

        [HttpPost(Name = "CreatePortfolio")]
        [Authorize(Roles = "Craftsman")]
        [RequireVerifiedCraftsman]
        public async Task<IActionResult> CreatePortfolio([FromForm] CreatePortfolioDto dto, [FromForm] IEnumerable<IFormFile> images)
        {
            var craftsmanId = GetCurrentUserId();
            var response = await _portfolioService.CreatePortfolioAsync(dto, images, craftsmanId);
            return HandleResponse(response);
        }

        [HttpPut("{id}", Name = "UpdatePortfolio")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> UpdatePortfolio(string id, [FromBody] UpdatePortfolioDto dto)
        {
            var craftsmanId = GetCurrentUserId();
            var response = await _portfolioService.UpdatePortfolioAsync(id, dto, craftsmanId);
            return HandleResponse(response);
        }

        [HttpDelete("{id}", Name = "DeletePortfolio")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> DeletePortfolio(string id)
        {
            var craftsmanId = GetCurrentUserId();
            var response = await _portfolioService.DeletePortfolioAsync(id, craftsmanId);
            return HandleResponse(response);
        }

        [HttpPost("{id}/images", Name = "AddImages")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> AddImages(string id, [FromForm] IEnumerable<IFormFile> images)
        {
            var craftsmanId = GetCurrentUserId();
            var response = await _portfolioService.AddImagesToPortfolioAsync(id, images, craftsmanId);
            return HandleResponse(response);
        }

        [HttpDelete("{portfolioId}/images/{imageId}", Name = "RemoveImage")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> RemoveImage(string portfolioId, string imageId)
        {
            var craftsmanId = GetCurrentUserId();
            var response = await _portfolioService.RemoveImageFromPortfolioAsync(portfolioId, imageId, craftsmanId);
            return HandleResponse(response);
        }

        [HttpPost("request", Name = "CreatePortfolioRequest")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreatePortfolioRequest([FromBody] CreatePortfolioRequestDto dto)
        {
            var customerId = GetCurrentUserId();
            var response = await _portfolioService.CreatePortfolioRequestAsync(dto, customerId);
            return HandleResponse(response);
        }

        [HttpPost("request/{requestId}/respond", Name = "RespondToPortfolioRequest")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> RespondToPortfolioRequest(string requestId, [FromBody] bool accept)
        {
            var craftsmanId = GetCurrentUserId();
            var response = await _portfolioService.RespondToPortfolioRequestAsync(requestId, craftsmanId, accept);
            return HandleResponse(response);
        }

        [HttpGet("requests/customer", Name = "GetCustomerRequests")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCustomerRequests()
        {
            var customerId = GetCurrentUserId();
            var response = await _portfolioService.GetCustomerPortfolioRequestsAsync(customerId);
            return HandleResponse(response);
        }

        [HttpGet("requests/craftsman", Name = "GetCraftsmanRequests")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> GetCraftsmanRequests()
        {
            var craftsmanId = GetCurrentUserId();
            var response = await _portfolioService.GetCraftsmanPortfolioRequestsAsync(craftsmanId);
            return HandleResponse(response);
        }
    }
}
