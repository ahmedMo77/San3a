using AutoMapper;
using San3a.Core.DTOs.Craftsman;
using San3a.Core.DTOs.Job;
using San3a.Core.DTOs.Offer;
using San3a.Core.DTOs.Service;
using San3a.Core.DTOs.Portfolio;
using San3a.Core.Entities;
using San3a.Core.DTOs.auth;
using San3a.Core.DTOs.Customer;

namespace San3a.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        #region Constructors
        public MappingProfile()
        {
            // Auth mappings
            CreateMap<RegisterAppUserDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => 
                    !string.IsNullOrWhiteSpace(src.Email) && src.Email.Contains('@') 
                        ? src.Email.Substring(0, src.Email.IndexOf('@')) 
                        : src.Email));

            CreateMap<RegisterCraftsmanDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => 
                    !string.IsNullOrWhiteSpace(src.Email) && src.Email.Contains('@') 
                        ? src.Email.Substring(0, src.Email.IndexOf('@')) 
                        : src.Email));

            CreateMap<RegisterAdminDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => 
                    !string.IsNullOrWhiteSpace(src.Email) && src.Email.Contains('@') 
                        ? src.Email.Substring(0, src.Email.IndexOf('@')) 
                        : src.Email));

            // Craftsman mappings
            CreateMap<Craftsman, CraftsmanResponseDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.AppUser.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.AppUser.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.PhoneNumber))
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.AppUser.Governorate))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.AppUser.ProfileImageUrl))
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewCount, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedJobsCount, opt => opt.MapFrom(src => src.AcceptedJobs.Count(j => j.Status == Core.Enums.JobStatus.Completed)));

            CreateMap<CreateCraftsmanDto, Craftsman>();
            CreateMap<UpdateCraftsmanDto, Craftsman>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Customer mappings
            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.AppUser.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.AppUser.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.PhoneNumber))
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.AppUser.Governorate))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.AppUser.ProfileImageUrl))
                .ForMember(dest => dest.JobsCount, opt => opt.MapFrom(src => src.Jobs.Count))
                .ForMember(dest => dest.ReviewsCount, opt => opt.Ignore());

            // Job mappings
            CreateMap<Job, JobResponseDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.AppUser.FullName))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceType.Name))
                .ForMember(dest => dest.AcceptedCraftsmanName, opt => opt.MapFrom(src => src.AcceptedWorker != null ? src.AcceptedWorker.AppUser.FullName : null))
                .ForMember(dest => dest.DirectCraftsmanName, opt => opt.MapFrom(src => src.DirectCraftsman != null ? src.DirectCraftsman.AppUser.FullName : null))
                .ForMember(dest => dest.OffersCount, opt => opt.MapFrom(src => src.Offers.Count))
                .ForMember(dest => dest.DirectRequestsCount, opt => opt.MapFrom(src => src.DirectRequests.Count));

            CreateMap<CreateJobDto, Job>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Core.Enums.JobStatus.Open))
                .ForMember(dest => dest.PostingType, opt => opt.MapFrom(src => Core.Enums.PostingType.Public));

            CreateMap<CreateJobWithDirectRequestDto, Job>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Core.Enums.JobStatus.Open));

            CreateMap<UpdateJobDto, Job>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Offer mappings
            CreateMap<Offer, OfferResponseDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job.Title))
                .ForMember(dest => dest.CraftsmanName, opt => opt.MapFrom(src => src.Worker.AppUser.FullName))
                .ForMember(dest => dest.CraftsmanServiceName, opt => opt.MapFrom(src => src.Worker.Service.Name));

            CreateMap<CreateOfferDto, Offer>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Core.Enums.OfferStatus.Pending));

            CreateMap<UpdateOfferDto, Offer>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Service mappings
            CreateMap<Service, ServiceResponseDto>()
                .ForMember(dest => dest.CraftsmenCount, opt => opt.MapFrom(src => src.Craftsmen.Count))
                .ForMember(dest => dest.JobsCount, opt => opt.MapFrom(src => src.Jobs.Count));

            CreateMap<CreateServiceDto, Service>();

            CreateMap<UpdateServiceDto, Service>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Portfolio mappings
            CreateMap<CraftsmanPortfolio, PortfolioResponseDto>()
                .ForMember(dest => dest.CraftsmanName, opt => opt.MapFrom(src => src.Craftsman.AppUser.FullName))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

            CreateMap<CraftsmanPortfolioImage, PortfolioImageDto>();

            CreateMap<CreatePortfolioDto, CraftsmanPortfolio>();

            CreateMap<UpdatePortfolioDto, CraftsmanPortfolio>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Portfolio Request mappings
            CreateMap<PortfolioRequest, PortfolioRequestResponseDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.AppUser.FullName))
                .ForMember(dest => dest.Portfolio, opt => opt.MapFrom(src => src.Portfolio));

            CreateMap<CreatePortfolioRequestDto, PortfolioRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Core.Enums.OfferStatus.Pending));
        }
        #endregion
    }
}
