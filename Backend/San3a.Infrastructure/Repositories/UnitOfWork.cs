using Microsoft.EntityFrameworkCore.Storage;
using San3a.Core.Interfaces;
using San3a.Infrastructure.Data;

namespace San3a.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public IJobRepository Jobs { get; }
        public ICustomerRepository Customers { get; }
        public ICraftsmanRepository Craftsmen { get; }
        public IServiceRepository Services { get; }
        public IOfferRepository Offers { get; }
        public IJobRequestRepository JobRequests { get; }
        public ICraftsmanPortfolioRepository Portfolios { get; }
        public IPortfolioRequestRepository PortfolioRequests { get; }
        public IFileUploadRepository FileUploads { get; }

        public UnitOfWork(
            AppDbContext context,
            IJobRepository jobs,
            ICustomerRepository customers,
            ICraftsmanRepository craftsmen,
            IServiceRepository services,
            IOfferRepository offers,
            IJobRequestRepository jobRequests,
            ICraftsmanPortfolioRepository portfolios,
            IPortfolioRequestRepository portfolioRequests,
            IFileUploadRepository fileUploads)
        {
            _context = context;
            Jobs = jobs;
            Customers = customers;
            Craftsmen = craftsmen;
            Services = services;
            Offers = offers;
            JobRequests = jobRequests;
            Portfolios = portfolios;
            PortfolioRequests = portfolioRequests;
            FileUploads = fileUploads;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
