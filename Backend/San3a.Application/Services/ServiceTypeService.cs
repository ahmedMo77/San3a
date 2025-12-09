using AutoMapper;
using San3a.Application.Interfaces;
using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Service;
using San3a.Core.Entities;
using San3a.Core.Interfaces;

namespace San3a.Application.Services
{
    public class ServiceTypeService : BaseService<Service, ServiceResponseDto, CreateServiceDto, UpdateServiceDto>, IServiceTypeService
    {
        #region Fields
        private readonly IServiceRepository _serviceRepository;
        #endregion

        #region Constructors
        public ServiceTypeService(IServiceRepository serviceRepository, IMapper mapper) 
            : base(serviceRepository, mapper)
        {
            _serviceRepository = serviceRepository;
        }
        #endregion

        #region Public Methods
        public async Task<ApiResponse<ServiceResponseDto>> GetServiceWithCraftsmenAsync(string id)
        {
            try
            {
                var service = await _serviceRepository.GetServiceWithCraftsmenAsync(id);
                if (service == null)
                {
                    return ApiResponse<ServiceResponseDto>.FailureResponse(
                        "Service not found",
                        new List<string> { $"No service found with ID: {id}" }
                    );
                }

                var dto = _mapper.Map<ServiceResponseDto>(service);
                return ApiResponse<ServiceResponseDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<ServiceResponseDto>.FailureResponse(
                    "An error occurred while retrieving service details",
                    new List<string> { ex.Message }
                );
            }
        }

        public override async Task<ApiResponse<ServiceResponseDto>> CreateAsync(CreateServiceDto dto, string userId)
        {
            try
            {
                // Validate input
                if (dto == null)
                {
                    return ApiResponse<ServiceResponseDto>.FailureResponse(
                        "Invalid request",
                        new List<string> { "Service data cannot be null" }
                    );
                }

                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return ApiResponse<ServiceResponseDto>.FailureResponse(
                        "Validation failed",
                        new List<string> { "Service name is required" }
                    );
                }

                // Check if service with same name already exists
                var allServices = await _serviceRepository.GetAllAsync();
                if (allServices.Any(s => s.Name.ToLower() == dto.Name.ToLower()))
                {
                    return ApiResponse<ServiceResponseDto>.FailureResponse(
                        "Service already exists",
                        new List<string> { $"A service with the name '{dto.Name}' already exists" }
                    );
                }

                return await base.CreateAsync(dto, userId);
            }
            catch (Exception ex)
            {
                return ApiResponse<ServiceResponseDto>.FailureResponse(
                    "An error occurred while creating the service",
                    new List<string> { ex.Message, ex.InnerException?.Message ?? "No inner exception" }
                );
            }
        }
        #endregion
    }
}
