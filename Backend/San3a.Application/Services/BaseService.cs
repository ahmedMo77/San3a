using AutoMapper;
using San3a.Application.Interfaces;
using San3a.Core.Base;
using San3a.Core.DTOs.Base;
using San3a.Core.Interfaces;
using San3a.Core.Specifications;

namespace San3a.Application.Services
{
    public abstract class BaseService<TEntity, TResponseDto, TCreateDto, TUpdateDto> 
        : IBaseService<TResponseDto, TCreateDto, TUpdateDto>
        where TEntity : BaseEntity
        where TResponseDto : class
        where TCreateDto : class
        where TUpdateDto : class
    {
        #region Fields
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        #endregion

        #region Constructors
        protected BaseService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods
        public virtual async Task<ApiResponse<TResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    return ApiResponse<TResponseDto>.FailureResponse(
                        $"{typeof(TEntity).Name} not found",
                        new List<string> { $"No {typeof(TEntity).Name} found with ID: {id}" }
                    );
                }

                var dto = _mapper.Map<TResponseDto>(entity);
                return ApiResponse<TResponseDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<TResponseDto>.FailureResponse(
                    "An error occurred while retrieving the data",
                    new List<string> { ex.Message }
                );
            }
        }

        public virtual async Task<ApiResponse<List<TResponseDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                var dtos = _mapper.Map<List<TResponseDto>>(entities);
                return ApiResponse<List<TResponseDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TResponseDto>>.FailureResponse(
                    "An error occurred while retrieving the data",
                    new List<string> { ex.Message }
                );
            }
        }

        public virtual async Task<ApiResponse<PagedResponse<TResponseDto>>> GetPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                var countSpec = new BaseSpecification<TEntity>();
                var totalCount = await _repository.CountAsync(countSpec);

                var pagedSpec = new BaseSpecification<TEntity>();
                pagedSpec.GetType()
                    .GetMethod("ApplyPaging", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.Invoke(pagedSpec, new object[] { (pageNumber - 1) * pageSize, pageSize });

                var entities = await _repository.ListAsync(pagedSpec);
                var dtos = _mapper.Map<List<TResponseDto>>(entities);

                var pagedResponse = new PagedResponse<TResponseDto>(dtos, pageNumber, pageSize, totalCount);
                return ApiResponse<PagedResponse<TResponseDto>>.SuccessResponse(pagedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<TResponseDto>>.FailureResponse(
                    "An error occurred while retrieving the data",
                    new List<string> { ex.Message }
                );
            }
        }

        public virtual async Task<ApiResponse<PagedResponse<TResponseDto>>> GetPagedAsync(PaginationParams pagination)
        {
            return await GetPagedAsync(pagination.PageNumber, pagination.PageSize);
        }

        public virtual async Task<ApiResponse<TResponseDto>> CreateAsync(TCreateDto dto, string userId)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(dto);
                
                // Ensure the entity has an ID (in case AutoMapper didn't trigger the constructor)
                if (string.IsNullOrEmpty(entity.Id))
                {
                    entity.Id = Guid.NewGuid().ToString();
                }
                
                var createdEntity = await _repository.AddAsync(entity);
                var responseDto = _mapper.Map<TResponseDto>(createdEntity);
                
                return ApiResponse<TResponseDto>.SuccessResponse(
                    responseDto,
                    $"{typeof(TEntity).Name} created successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<TResponseDto>.FailureResponse(
                    "An error occurred while creating the data",
                    new List<string> { ex.Message, ex.InnerException?.Message }
                );
            }
        }

        public virtual async Task<ApiResponse<TResponseDto>> UpdateAsync(string id, TUpdateDto dto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    return ApiResponse<TResponseDto>.FailureResponse(
                        $"{typeof(TEntity).Name} not found",
                        new List<string> { $"No {typeof(TEntity).Name} found with ID: {id}" }
                    );
                }

                _mapper.Map(dto, entity);
                await _repository.UpdateAsync(entity);
                
                var responseDto = _mapper.Map<TResponseDto>(entity);
                return ApiResponse<TResponseDto>.SuccessResponse(
                    responseDto,
                    $"{typeof(TEntity).Name} updated successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<TResponseDto>.FailureResponse(
                    "An error occurred while updating the data",
                    new List<string> { ex.Message }
                );
            }
        }

        public virtual async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    return ApiResponse<bool>.FailureResponse(
                        $"{typeof(TEntity).Name} not found",
                        new List<string> { $"No {typeof(TEntity).Name} found with ID: {id}" }
                    );
                }

                await _repository.DeleteAsync(entity);
                return ApiResponse<bool>.SuccessResponse(
                    true,
                    $"{typeof(TEntity).Name} deleted successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse(
                    "An error occurred while deleting the data",
                    new List<string> { ex.Message }
                );
            }
        }
        #endregion
    }
}
