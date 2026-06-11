using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Interfaces
{
    public interface IFileService
    {
        Task DeleteAsync(string filePath);
        Task<(string storedPath, string fileName)> SaveAsync(Stream fileStream, string originalFileName, string folder);
    }
}
