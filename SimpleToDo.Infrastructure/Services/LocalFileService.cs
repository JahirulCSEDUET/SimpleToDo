using SimpleToDo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Infrastructure.Services
{
   
    public class LocalFileService : IFileService
    {
        private readonly string _webRootPath;
        private readonly string[] _allowedExtensions = [".pdf", ".jpg", ".jpeg", ".png", ".zip"];
        private const long MaxFileSizeBytes = 5 * 1024 * 1024;
        public LocalFileService(string webRootPath)
        {
            _webRootPath = webRootPath;
        }       

        public async Task<(string storedPath, string fileName)> SaveAsync(Stream fileStream, string originalFileName, string folder)
        {
            var ext = Path.GetExtension(originalFileName).ToLowerInvariant();

            if (!_allowedExtensions.Contains(ext))
                throw new InvalidOperationException($"File type {ext} is not allowed.");

            if (fileStream.Length > MaxFileSizeBytes)
                throw new InvalidOperationException("File exceeds 5 MB limit.");

            var safeFileName = $"{Guid.NewGuid()}{ext}";
            var uploadDir = Path.Combine(_webRootPath, "uploads", folder);
            Directory.CreateDirectory(uploadDir);

            var fullPath = Path.Combine(uploadDir, safeFileName);
            await using var output = new FileStream(fullPath, FileMode.Create);
            await fileStream.CopyToAsync(output);

            return (Path.Combine("uploads", folder, safeFileName), originalFileName);
        }
        public Task DeleteAsync(string storedPath)
        {
            var fullPath = Path.Combine(_webRootPath, storedPath);
            if (File.Exists(fullPath)) File.Delete(fullPath);
            return Task.CompletedTask;
        }
    }
}
