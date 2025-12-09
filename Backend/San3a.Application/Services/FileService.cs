using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using San3a.Application.Interfaces;

namespace San3a.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadsPath;
        private const long DefaultMaxFileSize = 5242880;
        private readonly string[] _defaultAllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".doc", ".docx" };

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(_uploadsPath))
            {
                Directory.CreateDirectory(_uploadsPath);
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName, string? subFolder = null)
        {
            if (!ValidateFile(file))
            {
                throw new InvalidOperationException("Invalid file");
            }

            var folderPath = string.IsNullOrEmpty(subFolder)
                ? Path.Combine(_uploadsPath, folderName)
                : Path.Combine(_uploadsPath, folderName, subFolder);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var safeFileName = GetSafeFileName(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}_{safeFileName}";
            var filePath = Path.Combine(folderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = string.IsNullOrEmpty(subFolder)
                ? $"/uploads/{folderName}/{uniqueFileName}"
                : $"/uploads/{folderName}/{subFolder}/{uniqueFileName}";

            return relativePath;
        }

        public async Task<List<string>> UploadMultipleFilesAsync(IEnumerable<IFormFile> files, string folderName, string? subFolder = null)
        {
            var uploadedFiles = new List<string>();

            foreach (var file in files)
            {
                try
                {
                    var fileUrl = await UploadFileAsync(file, folderName, subFolder);
                    uploadedFiles.Add(fileUrl);
                }
                catch
                {
                    foreach (var uploadedFile in uploadedFiles)
                    {
                        await DeleteFileAsync(uploadedFile);
                    }
                    throw;
                }
            }

            return uploadedFiles;
        }

        public Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUrl))
                {
                    return Task.FromResult(false);
                }

                var filePath = Path.Combine(_environment.WebRootPath, fileUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public async Task<bool> DeleteMultipleFilesAsync(IEnumerable<string> fileUrls)
        {
            var allDeleted = true;

            foreach (var fileUrl in fileUrls)
            {
                var deleted = await DeleteFileAsync(fileUrl);
                if (!deleted)
                {
                    allDeleted = false;
                }
            }

            return allDeleted;
        }

        public bool ValidateFile(IFormFile file, long maxSizeInBytes = DefaultMaxFileSize, string[]? allowedExtensions = null)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            if (file.Length > maxSizeInBytes)
            {
                return false;
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var extensionsToCheck = allowedExtensions ?? _defaultAllowedExtensions;

            if (!extensionsToCheck.Contains(extension))
            {
                return false;
            }

            return true;
        }

        public string GetSafeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var safeName = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
            return safeName;
        }
    }
}
