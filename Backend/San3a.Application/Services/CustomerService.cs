using AutoMapper;
using San3a.Application.Interfaces;
using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Customer;
using San3a.Core.DTOs.Job;
using San3a.Core.Entities;
using San3a.Core.Interfaces;

namespace San3a.Application.Services
{
    public class CustomerService : BaseService<Customer, CustomerResponseDto, object, object>, ICustomerService
    {
        #region Fields
        private readonly ICustomerRepository _customerRepository;
        #endregion

        #region Constructors
        public CustomerService(ICustomerRepository customerRepository, IMapper mapper) 
            : base(customerRepository, mapper)
        {
            _customerRepository = customerRepository;
        }
        #endregion

        #region Public Methods
        public async Task<ApiResponse<CustomerResponseDto>> GetCustomerWithDetailsAsync(string id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerWithDetailsAsync(id);
                if (customer == null)
                {
                    return ApiResponse<CustomerResponseDto>.FailureResponse(
                        "Customer not found",
                        new List<string> { $"No customer found with ID: {id}" }
                    );
                }

                var dto = _mapper.Map<CustomerResponseDto>(customer);
                return ApiResponse<CustomerResponseDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<CustomerResponseDto>.FailureResponse(
                    "An error occurred while retrieving customer details",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<List<JobResponseDto>>> GetCustomerJobsAsync(string customerId)
        {
            try
            {
                var jobs = await _customerRepository.GetCustomerJobsAsync(customerId);
                var dtos = _mapper.Map<List<JobResponseDto>>(jobs);
                return ApiResponse<List<JobResponseDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<JobResponseDto>>.FailureResponse(
                    "An error occurred while retrieving customer jobs",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<CustomerResponseDto>> CreateCustomerAsync(string userId)
        {
            try
            {
                var customer = new Customer
                {
                    Id = userId
                };

                var createdCustomer = await _customerRepository.AddAsync(customer);
                var dto = await GetCustomerWithDetailsAsync(createdCustomer.Id);

                return dto;
            }
            catch (Exception ex)
            {
                return ApiResponse<CustomerResponseDto>.FailureResponse(
                    "An error occurred while creating customer profile",
                    new List<string> { ex.Message }
                );
            }
        }
        #endregion
    }
}
