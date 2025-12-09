using Microsoft.EntityFrameworkCore;
using San3a.Core.Entities;
using San3a.Core.Interfaces;
using San3a.Infrastructure.Data;

namespace San3a.Infrastructure.Repositories
{
    public class FileUploadRepository : GenericRepository<FileUpload>, IFileUploadRepository
    {
        private readonly AppDbContext _context;

        public FileUploadRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<FileUpload>> GetByEntityAsync(string entityType, string entityId)
        {
            return await _context.Set<FileUpload>()
                .Where(f => f.EntityType == entityType && f.EntityId == entityId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
