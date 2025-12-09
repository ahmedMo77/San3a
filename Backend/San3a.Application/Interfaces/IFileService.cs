using Microsoft.AspNetCore.Http;

namespace San3a.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName, string? subFolder = null);
        Task<List<string>> UploadMultipleFilesAsync(IEnumerable<IFormFile> files, string folderName, string? subFolder = null);
        Task<bool> DeleteFileAsync(string fileUrl);
        Task<bool> DeleteMultipleFilesAsync(IEnumerable<string> fileUrls);
        bool ValidateFile(IFormFile file, long maxSizeInBytes = 5242880, string[]? allowedExtensions = null);
        string GetSafeFileName(string fileName);
    }
}
