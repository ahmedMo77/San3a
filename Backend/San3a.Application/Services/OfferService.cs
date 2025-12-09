using AutoMapper;
using San3a.Application.Interfaces;
using San3a.Core.DTOs.Base;
using San3a.Core.DTOs.Offer;
using San3a.Core.Entities;
using San3a.Core.Enums;
using San3a.Core.Interfaces;

namespace San3a.Application.Services
{
    public class OfferService : BaseService<Offer, OfferResponseDto, CreateOfferDto, UpdateOfferDto>, IOfferService
    {
        #region Fields
        private readonly IOfferRepository _offerRepository;
        private readonly IJobRepository _jobRepository;
        #endregion

        #region Constructors
        public OfferService(
            IOfferRepository offerRepository,
            IJobRepository jobRepository,
            IMapper mapper) 
            : base(offerRepository, mapper)
        {
            _offerRepository = offerRepository;
            _jobRepository = jobRepository;
        }
        #endregion

        #region Public Methods
        public async Task<ApiResponse<List<OfferResponseDto>>> GetOffersByJobIdAsync(string jobId)
        {
            try
            {
                var offers = await _offerRepository.GetOffersByJobIdAsync(jobId);
                var dtos = _mapper.Map<List<OfferResponseDto>>(offers);
                return ApiResponse<List<OfferResponseDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<OfferResponseDto>>.FailureResponse(
                    "An error occurred while retrieving offers for the job",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<List<OfferResponseDto>>> GetOffersByCraftsmanIdAsync(string craftsmanId)
        {
            try
            {
                var offers = await _offerRepository.GetOffersByCraftsmanIdAsync(craftsmanId);
                var dtos = _mapper.Map<List<OfferResponseDto>>(offers);
                return ApiResponse<List<OfferResponseDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<OfferResponseDto>>.FailureResponse(
                    "An error occurred while retrieving craftsman offers",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<OfferResponseDto>> GetOfferWithDetailsAsync(string id)
        {
            try
            {
                var offer = await _offerRepository.GetOfferWithDetailsAsync(id);
                if (offer == null)
                {
                    return ApiResponse<OfferResponseDto>.FailureResponse(
                        "Offer not found",
                        new List<string> { $"No offer found with ID: {id}" }
                    );
                }

                var dto = _mapper.Map<OfferResponseDto>(offer);
                return ApiResponse<OfferResponseDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<OfferResponseDto>.FailureResponse(
                    "An error occurred while retrieving offer details",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<bool>> AcceptOfferAsync(string offerId)
        {
            try
            {
                var offer = await _offerRepository.GetOfferWithDetailsAsync(offerId);
                if (offer == null)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Offer not found",
                        new List<string> { $"No offer found with ID: {offerId}" }
                    );
                }

                // Check if job is still available
                var job = await _jobRepository.GetByIdAsync(offer.JobId);
                if (job == null)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Job not found",
                        new List<string> { "The associated job was not found" }
                    );
                }

                if (job.Status != JobStatus.Open && job.Status != JobStatus.Pending)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Job is no longer available",
                        new List<string> { "This job has already been assigned or completed" }
                    );
                }

                // Accept the offer
                offer.Status = OfferStatus.Accepted;
                await _offerRepository.UpdateAsync(offer);

                // Assign job to craftsman
                job.AcceptedCraftsmanId = offer.CraftsmanId;
                job.Status = JobStatus.Accepted;
                await _jobRepository.UpdateAsync(job);

                // Reject all other offers for this job
                var otherOffers = await _offerRepository.GetOffersByJobIdAsync(offer.JobId);
                foreach (var otherOffer in otherOffers.Where(o => o.Id != offerId && o.Status == OfferStatus.Pending))
                {
                    otherOffer.Status = OfferStatus.Rejected;
                    await _offerRepository.UpdateAsync(otherOffer);
                }

                return ApiResponse<bool>.SuccessResponse(true, "Offer accepted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse(
                    "An error occurred while accepting the offer",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<ApiResponse<bool>> RejectOfferAsync(string offerId)
        {
            try
            {
                var offer = await _offerRepository.GetByIdAsync(offerId);
                if (offer == null)
                {
                    return ApiResponse<bool>.FailureResponse(
                        "Offer not found",
                        new List<string> { $"No offer found with ID: {offerId}" }
                    );
                }

                offer.Status = OfferStatus.Rejected;
                await _offerRepository.UpdateAsync(offer);

                return ApiResponse<bool>.SuccessResponse(true, "Offer rejected successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse(
                    "An error occurred while rejecting the offer",
                    new List<string> { ex.Message }
                );
            }
        }

        public override async Task<ApiResponse<OfferResponseDto>> CreateAsync(CreateOfferDto dto, string userId)
        {
            try
            {
                // Check if job exists and is available
                var job = await _jobRepository.GetByIdAsync(dto.JobId);
                if (job == null)
                {
                    return ApiResponse<OfferResponseDto>.FailureResponse(
                        "Job not found",
                        new List<string> { $"No job found with ID: {dto.JobId}" }
                    );
                }

                if (job.Status != JobStatus.Open && job.Status != JobStatus.Pending)
                {
                    return ApiResponse<OfferResponseDto>.FailureResponse(
                        "Job is no longer available",
                        new List<string> { "This job has already been assigned or completed" }
                    );
                }

                // Check if craftsman already submitted an offer for this job
                var existingOffers = await _offerRepository.GetOffersByJobIdAsync(dto.JobId);
                if (existingOffers.Any(o => o.CraftsmanId == userId))
                {
                    return ApiResponse<OfferResponseDto>.FailureResponse(
                        "Offer already exists",
                        new List<string> { "You have already submitted an offer for this job" }
                    );
                }

                var offer = _mapper.Map<Offer>(dto);
                offer.CraftsmanId = userId;
                offer.Status = OfferStatus.Pending;

                var createdOffer = await _offerRepository.AddAsync(offer);
                var response = await GetOfferWithDetailsAsync(createdOffer.Id);

                return response;
            }
            catch (Exception ex)
            {
                return ApiResponse<OfferResponseDto>.FailureResponse(
                    "An error occurred while creating the offer",
                    new List<string> { ex.Message }
                );
            }
        }
        #endregion
    }
}
