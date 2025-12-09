namespace San3a.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IJobRepository Jobs { get; }
        ICustomerRepository Customers { get; }
        ICraftsmanRepository Craftsmen { get; }
        IServiceRepository Services { get; }
        IOfferRepository Offers { get; }
        IJobRequestRepository JobRequests { get; }
        ICraftsmanPortfolioRepository Portfolios { get; }
        IPortfolioRequestRepository PortfolioRequests { get; }
        IFileUploadRepository FileUploads { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
