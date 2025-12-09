using AutoMapper;
using San3a.Application.Interfaces;
using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Job;
using San3a.Core.Entities;
using San3a.Core.Enums;
using San3a.Core.Interfaces;

namespace San3a.Application.Services
{
    public class JobService : BaseService<Job, JobResponseDto, CreateJobDto, UpdateJobDto>, IJobService
    {
        #region Fields
        private readonly IJobRepository _jobRepository;
        private readonly ICraftsmanRepository _craftsmanRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IJobRequestRepository _jobRequestRepository;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructors
        public JobService(
            IJobRepository jobRepository,
            ICraftsmanRepository craftsmanRepository,
            IOfferRepository offerRepository,
            IJobRequestRepository jobRequestRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper) 
            : base(jobRepository, mapper)
        {
            _jobRepository = jobRepository;
            _craftsmanRepository = craftsmanRepository;
            _offerRepository = offerRepository;
            _jobRequestRepository = jobRequestRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Public Methods
        public async Task<ApiResponse<List<JobResponseDto>>> GetJobsByStatusAsync(JobStatus status)
        {
            try
            {
                var jobs = await _jobRepository.GetJobsByStatusAsync(status);
                var dtos = _mapper.Map<List<JobResponseDto>>(jobs);
                return ApiResponse<List<JobResponseDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<JobResponseDto>>.FailureResponse(
                    "An error occurred while retrieving jobs by status",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<List<JobResponseDto>>> GetJobsByCustomerIdAsync(string customerId)
        {
            try
            {
                var jobs = await _jobRepository.GetJobsByCustomerIdAsync(customerId);
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

        public async Task<ApiResponse<List<JobResponseDto>>> GetJobsByServiceIdAsync(string serviceId)
        {
            try
            {
                var jobs = await _jobRepository.GetJobsByServiceIdAsync(serviceId);
                var dtos = _mapper.Map<List<JobResponseDto>>(jobs);
                return ApiResponse<List<JobResponseDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<JobResponseDto>>.FailureResponse(
                    "An error occurred while retrieving jobs by service",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<JobResponseDto>> GetJobWithDetailsAsync(string id)
        {
            try
            {
                var job = await _jobRepository.GetJobWithDetailsAsync(id);
                if (job == null)
                {
                    return ApiResponse<JobResponseDto>.FailureResponse(
                        "Job not found",
                        new List<string> { $"No job found with ID: {id}" }
                    );
                }

                var dto = _mapper.Map<JobResponseDto>(job);
                return ApiResponse<JobResponseDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<JobResponseDto>.FailureResponse(
                    "An error occurred while retrieving job details",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<JobResponseDto>> CreateJobAsync(CreateJobWithDirectRequestDto dto, string customerId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var postingType = PostingType.Public;
                if (!string.IsNullOrEmpty(dto.DirectCraftsmanId) && dto.IsPublic)
                {
                    postingType = PostingType.Hybrid;
                }
                else if (!string.IsNullOrEmpty(dto.DirectCraftsmanId) && !dto.IsPublic)
                {
                    postingType = PostingType.Direct;
                }

                var job = new Job
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Location = dto.Location,
                    Budget = dto.Budget,
                    ServiceId = dto.ServiceId,
                    CustomerId = customerId,
                    Status = JobStatus.Open,
                    PostingType = postingType,
                    DirectCraftsmanId = dto.DirectCraftsmanId
                };

                var createdJob = await _jobRepository.AddAsync(job);

                if (!string.IsNullOrEmpty(dto.DirectCraftsmanId))
                {
                    var craftsman = await _craftsmanRepository.GetByIdAsync(dto.DirectCraftsmanId);
                    if (craftsman == null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ApiResponse<JobResponseDto>.FailureResponse(
                            "Craftsman not found",
                            new List<string> { $"No craftsman found with ID: {dto.DirectCraftsmanId}" }
                        );
                    }

                    var jobRequest = new JobRequest
                    {
                        JobId = createdJob.Id,
                        CraftsmanId = dto.DirectCraftsmanId,
                        Status = OfferStatus.Pending
                    };

                    await _jobRequestRepository.AddAsync(jobRequest);
                }

                await _unitOfWork.CommitTransactionAsync();

                return await GetJobWithDetailsAsync(createdJob.Id);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<JobResponseDto>.FailureResponse(
                    "An error occurred while creating job",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<bool>> RespondToJobRequestAsync(string jobRequestId, string craftsmanId, bool accept)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var jobRequest = await _jobRequestRepository.GetByIdAsync(jobRequestId);
                if (jobRequest == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.FailureResponse(
                        "Job request not found",
                        new List<string> { $"No job request found with ID: {jobRequestId}" }
                    );
                }

                if (jobRequest.CraftsmanId != craftsmanId)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.FailureResponse(
                        "Unauthorized",
                        new List<string> { "You are not authorized to respond to this request" }
                    );
                }

                var job = await _jobRepository.GetByIdAsync(jobRequest.JobId);
                if (job == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.FailureResponse(
                        "Job not found",
                        new List<string> { $"No job found with ID: {jobRequest.JobId}" }
                    );
                }

                if (accept)
                {
                    jobRequest.Status = OfferStatus.Accepted;
                    job.AcceptedCraftsmanId = craftsmanId;
                    job.Status = JobStatus.Accepted;

                    await _jobRequestRepository.UpdateAsync(jobRequest);

                    var otherRequests = await _jobRequestRepository.GetByJobIdAsync(job.Id);
                    foreach (var request in otherRequests.Where(r => r.Id != jobRequestId))
                    {
                        request.Status = OfferStatus.Rejected;
                        await _jobRequestRepository.UpdateAsync(request);
                    }

                    var offers = await _offerRepository.GetOffersByJobIdAsync(job.Id);
                    foreach (var offer in offers)
                    {
                        offer.Status = OfferStatus.Rejected;
                        await _offerRepository.UpdateAsync(offer);
                    }
                }
                else
                {
                    jobRequest.Status = OfferStatus.Rejected;
                    await _jobRequestRepository.UpdateAsync(jobRequest);
                }

                await _jobRepository.UpdateAsync(job);
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse<bool>.SuccessResponse(true, accept ? "Job request accepted" : "Job request rejected");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.FailureResponse(
                    "An error occurred while responding to job request",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<bool>> AssignJobToCraftsmanAsync(string jobId, string craftsmanId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var job = await _jobRepository.GetByIdAsync(jobId);
                if (job == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.FailureResponse(
                        "Job not found",
                        new List<string> { $"No job found with ID: {jobId}" }
                    );
                }

                var craftsman = await _craftsmanRepository.GetByIdAsync(craftsmanId);
                if (craftsman == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<bool>.FailureResponse(
                        "Craftsman not found",
                        new List<string> { $"No craftsman found with ID: {craftsmanId}" }
                    );
                }

                job.AcceptedCraftsmanId = craftsmanId;
                job.Status = JobStatus.Accepted;
                await _jobRepository.UpdateAsync(job);

                var offers = await _offerRepository.GetOffersByJobIdAsync(jobId);
                foreach (var offer in offers)
                {
                    offer.Status = offer.CraftsmanId == craftsmanId ? OfferStatus.Accepted : OfferStatus.Rejected;
                    await _offerRepository.UpdateAsync(offer);
                }

                var jobRequests = await _jobRequestRepository.GetByJobIdAsync(jobId);
                foreach (var request in jobRequests)
                {
                    request.Status = request.CraftsmanId == craftsmanId ? OfferStatus.Accepted : OfferStatus.Rejected;
                    await _jobRequestRepository.UpdateAsync(request);
                }

                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Job assigned to craftsman successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.FailureResponse(
                    "An error occurred while assigning job to craftsman",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<bool>> UpdateJobStatusAsync(string jobId, JobStatus status)
        {
            try
            {
                var job = await _jobRepository.GetByIdAsync(jobId);
                if (job == null)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Job not found",
                        new List<string> { $"No job found with ID: {jobId}" }
                    );
                }

                job.Status = status;
                await _jobRepository.UpdateAsync(job);

                return ApiResponse<bool>.SuccessResponse(true, "Job status updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse(
                    "An error occurred while updating job status",
                    new List<string> { ex.Message }
                );
            }
        }

        public override async Task<ApiResponse<JobResponseDto>> CreateAsync(CreateJobDto dto, string userId)
        {
            try
            {
                var job = _mapper.Map<Job>(dto);
                job.CustomerId = userId;
                job.Status = JobStatus.Pending;

                var createdJob = await _jobRepository.AddAsync(job);
                var response = await GetJobWithDetailsAsync(createdJob.Id);

                return response;
            }
            catch (Exception ex)
            {
                return ApiResponse<JobResponseDto>.FailureResponse(
                    "An error occurred while creating job",
                    new List<string> { ex.Message }
                );
            }
        }
        #endregion
    }
}
