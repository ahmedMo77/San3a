using AutoMapper;
using Microsoft.AspNetCore.Http;
using San3a.Application.Interfaces;
using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Portfolio;
using San3a.Core.Entities;
using San3a.Core.Enums;
using San3a.Core.Interfaces;

namespace San3a.Application.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ICraftsmanPortfolioRepository _portfolioRepository;
        private readonly IPortfolioRequestRepository _requestRepository;
        private readonly ICraftsmanRepository _craftsmanRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PortfolioService(
            ICraftsmanPortfolioRepository portfolioRepository,
            IPortfolioRequestRepository requestRepository,
            ICraftsmanRepository craftsmanRepository,
            ICustomerRepository customerRepository,
            IFileService fileService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _portfolioRepository = portfolioRepository;
            _requestRepository = requestRepository;
            _craftsmanRepository = craftsmanRepository;
            _customerRepository = customerRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PortfolioResponseDto>> CreatePortfolioAsync(CreatePortfolioDto dto, IEnumerable<IFormFile> images, string craftsmanId)
        {
            try
            {
                var craftsman = await _craftsmanRepository.GetByIdAsync(craftsmanId);
                if (craftsman == null)
                {
                    return ApiResponse<PortfolioResponseDto>.FailureResponse(
                        "Craftsman not found",
                        new List<string> { $"No craftsman found with ID: {craftsmanId}" }
                    );
                }

                if (!craftsman.IsVerified)
                {
                    return ApiResponse<PortfolioResponseDto>.FailureResponse(
                        "Unauthorized",
                        new List<string> { "Only verified craftsmen can upload portfolio items" }
                    );
                }

                if (images == null || !images.Any())
                {
                    return ApiResponse<PortfolioResponseDto>.FailureResponse(
                        "Images required",
                        new List<string> { "At least one image is required to create a portfolio" }
                    );
                }

                if (!ValidateImages(images, out var validationError))
                {
                    return ApiResponse<PortfolioResponseDto>.FailureResponse(
                        "Invalid images",
                        new List<string> { validationError }
                    );
                }

                await _unitOfWork.BeginTransactionAsync();

                var portfolio = new CraftsmanPortfolio
                {
                    CraftsmanId = craftsmanId,
                    Title = dto.Title,
                    Description = dto.Description
                };

                var createdPortfolio = await _portfolioRepository.AddAsync(portfolio);

                var imageUrls = await _fileService.UploadMultipleFilesAsync(images, "portfolios", craftsmanId);
                
                var portfolioImages = imageUrls.Select((url, index) => new CraftsmanPortfolioImage
                {
                    PortfolioId = createdPortfolio.Id,
                    ImageUrl = url,
                    DisplayOrder = index
                }).ToList();

                foreach (var image in portfolioImages)
                {
                    createdPortfolio.Images.Add(image);
                }

                await _portfolioRepository.UpdateAsync(createdPortfolio);

                await _unitOfWork.CommitTransactionAsync();

                var result = await GetPortfolioByIdAsync(createdPortfolio.Id);
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<PortfolioResponseDto>.FailureResponse(
                    "An error occurred while creating portfolio",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<PortfolioResponseDto>> UpdatePortfolioAsync(string portfolioId, UpdatePortfolioDto dto, string craftsmanId)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetByIdAsync(portfolioId);
                if (portfolio == null)
                {
                    return ApiResponse<PortfolioResponseDto>.FailureResponse(
                        "Portfolio not found",
                        new List<string> { $"No portfolio found with ID: {portfolioId}" }
                    );
                }

                if (portfolio.CraftsmanId != craftsmanId)
                {
                    return ApiResponse<PortfolioResponseDto>.FailureResponse(
                        "Unauthorized",
                        new List<string> { "You are not authorized to update this portfolio" }
                    );
                }

                if (!string.IsNullOrEmpty(dto.Title))
                {
                    portfolio.Title = dto.Title;
                }

                if (!string.IsNullOrEmpty(dto.Description))
                {
                    portfolio.Description = dto.Description;
                }

                await _portfolioRepository.UpdateAsync(portfolio);

                var result = await GetPortfolioByIdAsync(portfolioId);
                return result;
            }
            catch (Exception ex)
            {
                return ApiResponse<PortfolioResponseDto>.FailureResponse(
                    "An error occurred while updating portfolio",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<bool>> DeletePortfolioAsync(string portfolioId, string craftsmanId)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetWithImagesForUpdateAsync(portfolioId);
                if (portfolio == null)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Portfolio not found",
                        new List<string> { $"No portfolio found with ID: {portfolioId}" }
                    );
                }

                if (portfolio.CraftsmanId != craftsmanId)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Unauthorized",
                        new List<string> { "You are not authorized to delete this portfolio" }
                    );
                }

                await _unitOfWork.BeginTransactionAsync();

                if (portfolio.Images.Any())
                {
                    var imageUrls = portfolio.Images.Select(i => i.ImageUrl).ToList();
                    await _fileService.DeleteMultipleFilesAsync(imageUrls);
                }

                await _portfolioRepository.DeleteAsync(portfolio);

                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Portfolio deleted successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.FailureResponse(
                    "An error occurred while deleting portfolio",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<PortfolioResponseDto>> GetPortfolioByIdAsync(string portfolioId)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetWithImagesAsync(portfolioId);
                if (portfolio == null)
                {
                    return ApiResponse<PortfolioResponseDto>.FailureResponse(
                        "Portfolio not found",
                        new List<string> { $"No portfolio found with ID: {portfolioId}" }
                    );
                }

                var dto = _mapper.Map<PortfolioResponseDto>(portfolio);
                return ApiResponse<PortfolioResponseDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<PortfolioResponseDto>.FailureResponse(
                    "An error occurred while retrieving portfolio",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<List<PortfolioResponseDto>>> GetPortfoliosByCraftsmanIdAsync(string craftsmanId)
        {
            try
            {
                var portfolios = await _portfolioRepository.GetByCraftsmanIdAsync(craftsmanId);
                var dtos = _mapper.Map<List<PortfolioResponseDto>>(portfolios);
                return ApiResponse<List<PortfolioResponseDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PortfolioResponseDto>>.FailureResponse(
                    "An error occurred while retrieving portfolios",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<List<PortfolioResponseDto>>> GetAllPortfoliosAsync()
        {
            try
            {
                var portfolios = await _portfolioRepository.GetAllWithImagesAsync();
                var dtos = _mapper.Map<List<PortfolioResponseDto>>(portfolios);
                return ApiResponse<List<PortfolioResponseDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PortfolioResponseDto>>.FailureResponse(
                    "An error occurred while retrieving portfolios",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<bool>> AddImagesToPortfolioAsync(string portfolioId, IEnumerable<IFormFile> images, string craftsmanId)
        {
            try
            {
                if (images == null || !images.Any())
                {
                    return ApiResponse<bool>.FailureResponse(
                        "No images provided",
                        new List<string> { "At least one image is required" }
                    );
                }

                if (!ValidateImages(images, out var validationError))
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Invalid images",
                        new List<string> { validationError }
                    );
                }

                var portfolio = await _portfolioRepository.GetWithImagesForUpdateAsync(portfolioId);
                if (portfolio == null)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Portfolio not found",
                        new List<string> { $"No portfolio found with ID: {portfolioId}" }
                    );
                }

                if (portfolio.CraftsmanId != craftsmanId)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Unauthorized",
                        new List<string> { "You are not authorized to update this portfolio" }
                    );
                }

                await _unitOfWork.BeginTransactionAsync();

                var imageUrls = await _fileService.UploadMultipleFilesAsync(images, "portfolios", craftsmanId);
                
                var currentMaxOrder = portfolio.Images.Any() ? portfolio.Images.Max(i => i.DisplayOrder) : -1;
                
                var portfolioImages = imageUrls.Select((url, index) => new CraftsmanPortfolioImage
                {
                    PortfolioId = portfolioId,
                    ImageUrl = url,
                    DisplayOrder = currentMaxOrder + index + 1
                }).ToList();

                foreach (var image in portfolioImages)
                {
                    portfolio.Images.Add(image);
                }

                await _portfolioRepository.UpdateAsync(portfolio);
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Images added successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.FailureResponse(
                    "An error occurred while adding images",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<bool>> RemoveImageFromPortfolioAsync(string portfolioId, string imageId, string craftsmanId)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetWithImagesForUpdateAsync(portfolioId);
                if (portfolio == null)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Portfolio not found",
                        new List<string> { $"No portfolio found with ID: {portfolioId}" }
                    );
                }

                if (portfolio.CraftsmanId != craftsmanId)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Unauthorized",
                        new List<string> { "You are not authorized to update this portfolio" }
                    );
                }

                var image = portfolio.Images.FirstOrDefault(i => i.Id == imageId);
                if (image == null)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Image not found",
                        new List<string> { $"No image found with ID: {imageId}" }
                    );
                }

                if (portfolio.Images.Count <= 1)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Cannot remove last image",
                        new List<string> { "Portfolio must have at least one image" }
                    );
                }

                await _unitOfWork.BeginTransactionAsync();

                await _fileService.DeleteFileAsync(image.ImageUrl);
                portfolio.Images.Remove(image);

                await _portfolioRepository.UpdateAsync(portfolio);
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Image removed successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.FailureResponse(
                    "An error occurred while removing image",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<PortfolioRequestResponseDto>> CreatePortfolioRequestAsync(CreatePortfolioRequestDto dto, string customerId)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetByIdAsync(dto.PortfolioId);
                if (portfolio == null)
                {
                    return ApiResponse<PortfolioRequestResponseDto>.FailureResponse(
                        "Portfolio not found",
                        new List<string> { $"No portfolio found with ID: {dto.PortfolioId}" }
                    );
                }

                var request = new PortfolioRequest
                {
                    PortfolioId = dto.PortfolioId,
                    CustomerId = customerId,
                    Message = dto.Message,
                    Status = OfferStatus.Pending
                };

                var createdRequest = await _requestRepository.AddAsync(request);

                var result = _mapper.Map<PortfolioRequestResponseDto>(createdRequest);
                return ApiResponse<PortfolioRequestResponseDto>.SuccessResponse(result, "Request created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PortfolioRequestResponseDto>.FailureResponse(
                    "An error occurred while creating portfolio request",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<bool>> RespondToPortfolioRequestAsync(string requestId, string craftsmanId, bool accept)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var request = await _requestRepository.GetByIdAsync(requestId);
                if (request == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.FailureResponse(
                        "Request not found",
                        new List<string> { $"No request found with ID: {requestId}" }
                    );
                }

                var portfolio = await _portfolioRepository.GetByIdAsync(request.PortfolioId);
                if (portfolio == null || portfolio.CraftsmanId != craftsmanId)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.FailureResponse(
                        "Unauthorized",
                        new List<string> { "You are not authorized to respond to this request" }
                    );
                }

                request.Status = accept ? OfferStatus.Accepted : OfferStatus.Rejected;
                await _requestRepository.UpdateAsync(request);

                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse<bool>.SuccessResponse(true, accept ? "Request accepted" : "Request rejected");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.FailureResponse(
                    "An error occurred while responding to request",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<List<PortfolioRequestResponseDto>>> GetCustomerPortfolioRequestsAsync(string customerId)
        {
            try
            {
                var requests = await _requestRepository.GetByCustomerIdAsync(customerId);
                var dtos = _mapper.Map<List<PortfolioRequestResponseDto>>(requests);
                return ApiResponse<List<PortfolioRequestResponseDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PortfolioRequestResponseDto>>.FailureResponse(
                    "An error occurred while retrieving requests",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<List<PortfolioRequestResponseDto>>> GetCraftsmanPortfolioRequestsAsync(string craftsmanId)
        {
            try
            {
                var portfolios = await _portfolioRepository.GetByCraftsmanIdAsync(craftsmanId);
                var allRequests = new List<PortfolioRequest>();

                foreach (var portfolio in portfolios)
                {
                    var requests = await _requestRepository.GetByPortfolioIdAsync(portfolio.Id);
                    allRequests.AddRange(requests);
                }

                var dtos = _mapper.Map<List<PortfolioRequestResponseDto>>(allRequests);
                return ApiResponse<List<PortfolioRequestResponseDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PortfolioRequestResponseDto>>.FailureResponse(
                    "An error occurred while retrieving requests",
                    new List<string> { ex.Message }
                );
            }
        }

        private bool ValidateImages(IEnumerable<IFormFile> images, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (images == null || !images.Any())
            {
                errorMessage = "No images provided";
                return false;
            }

            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            const long maxImageSize = 5242880; // 5MB

            foreach (var image in images)
            {
                if (image == null || image.Length == 0)
                {
                    errorMessage = "One or more images are empty";
                    return false;
                }

                if (image.Length > maxImageSize)
                {
                    errorMessage = $"Image {image.FileName} exceeds maximum size of 5MB";
                    return false;
                }

                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!allowedImageExtensions.Contains(extension))
                {
                    errorMessage = $"Image {image.FileName} has invalid extension. Only jpg, jpeg, png, gif, and webp are allowed";
                    return false;
                }
            }

            return true;
        }
    }
}
