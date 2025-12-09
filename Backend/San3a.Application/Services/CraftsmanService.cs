using AutoMapper;
using San3a.Application.Interfaces;
using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Craftsman;
using San3a.Core.Entities;
using San3a.Core.Interfaces;

namespace San3a.Application.Services
{
    public class CraftsmanService : BaseService<Craftsman, CraftsmanResponseDto, CreateCraftsmanDto, UpdateCraftsmanDto>, ICraftsmanService
    {
        #region Fields
        private readonly ICraftsmanRepository _craftsmanRepository;
        #endregion

        #region Constructors
        public CraftsmanService(
            ICraftsmanRepository craftsmanRepository,
            IMapper mapper) 
            : base(craftsmanRepository, mapper)
        {
            _craftsmanRepository = craftsmanRepository;
        }
        #endregion

        #region Public Methods
        public async Task<ApiResponse<List<CraftsmanResponseDto>>> GetCraftsmenByServiceIdAsync(string serviceId)
        {
            try
            {
                var craftsmen = await _craftsmanRepository.GetCraftsmenByServiceIdAsync(serviceId);
                var dtos = _mapper.Map<List<CraftsmanResponseDto>>(craftsmen);

                return ApiResponse<List<CraftsmanResponseDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CraftsmanResponseDto>>.FailureResponse(
                    "An error occurred while retrieving craftsmen by service",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<CraftsmanResponseDto>> GetCraftsmanWithDetailsAsync(string id)
        {
            try
            {
                var craftsman = await _craftsmanRepository.GetCraftsmanWithDetailsAsync(id);
                if (craftsman == null)
                {
                    return ApiResponse<CraftsmanResponseDto>.FailureResponse(
                        "Craftsman not found",
                        new List<string> { $"No craftsman found with ID: {id}" }
                    );
                }

                var dto = _mapper.Map<CraftsmanResponseDto>(craftsman);

                return ApiResponse<CraftsmanResponseDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<CraftsmanResponseDto>.FailureResponse(
                    "An error occurred while retrieving craftsman details",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<List<CraftsmanResponseDto>>> GetVerifiedCraftsmenAsync()
        {
            try
            {
                var craftsmen = await _craftsmanRepository.GetVerifiedCraftsmenAsync();
                var dtos = _mapper.Map<List<CraftsmanResponseDto>>(craftsmen);

                return ApiResponse<List<CraftsmanResponseDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CraftsmanResponseDto>>.FailureResponse(
                    "An error occurred while retrieving verified craftsmen",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<List<CraftsmanVerificationResponseDto>>> GetPendingVerificationCraftsmenAsync()
        {
            try
            {
                var craftsmen = await _craftsmanRepository.GetPendingVerificationCraftsmenAsync();
                var dtos = new List<CraftsmanVerificationResponseDto>();

                foreach (var craftsman in craftsmen)
                {
                    var dto = new CraftsmanVerificationResponseDto
                    {
                        CraftsmanId = craftsman.Id,
                        FullName = craftsman.AppUser.FullName,
                        Email = craftsman.AppUser.Email,
                        PhoneNumber = craftsman.AppUser.PhoneNumber,
                        NationalId = craftsman.NationalId,
                        ServiceName = craftsman.Service?.Name,
                        IsVerified = craftsman.IsVerified,
                        CreatedAt = craftsman.AppUser.CreatedAt
                    };
                    dtos.Add(dto);
                }

                return ApiResponse<List<CraftsmanVerificationResponseDto>>.SuccessResponse(
                    dtos,
                    $"Found {dtos.Count} pending verification requests"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CraftsmanVerificationResponseDto>>.FailureResponse(
                    "An error occurred while retrieving pending verification craftsmen",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<bool>> ApproveCraftsmanVerificationAsync(VerifyCraftsmanDto dto)
        {
            try
            {
                var craftsman = await _craftsmanRepository.GetByIdAsync(dto.CraftsmanId);
                if (craftsman == null)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Craftsman not found",
                        new List<string> { $"No craftsman found with ID: {dto.CraftsmanId}" }
                    );
                }

                if (dto.IsApproved)
                {
                    craftsman.IsVerified = true;
                    await _craftsmanRepository.UpdateAsync(craftsman);
                    return ApiResponse<bool>.SuccessResponse(true, "Craftsman verification approved successfully");
                }
                else
                {
                    return ApiResponse<bool>.SuccessResponse(
                        false, 
                        $"Craftsman verification rejected. Reason: {dto.RejectionReason ?? "Not specified"}"
                    );
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse(
                    "An error occurred while processing craftsman verification",
                    new List<string> { ex.Message }
                );
            }
        }

        public override async Task<ApiResponse<CraftsmanResponseDto>> CreateAsync(CreateCraftsmanDto dto, string userId)
        {
            try
            {
                var craftsman = new Craftsman
                {
                    Id = userId,
                    NationalId = dto.NationalId,
                    ServiceId = dto.ServiceId,
                    IsVerified = false
                };

                var createdCraftsman = await _craftsmanRepository.AddAsync(craftsman);
                var response = await GetCraftsmanWithDetailsAsync(createdCraftsman.Id);

                return response;
            }
            catch (Exception ex)
            {
                return ApiResponse<CraftsmanResponseDto>.FailureResponse(
                    "An error occurred while creating craftsman profile",
                    new List<string> { ex.Message }
                );
            }
        }
        #endregion
    }
}
