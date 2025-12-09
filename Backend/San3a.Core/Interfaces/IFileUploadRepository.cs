using San3a.Core.Entities;
using San3a.Core.Interfaces;

namespace San3a.Core.Interfaces
{
    public interface IFileUploadRepository : IGenericRepository<FileUpload>
    {
        Task<IReadOnlyList<FileUpload>> GetByEntityAsync(string entityType, string entityId);
    }
}
