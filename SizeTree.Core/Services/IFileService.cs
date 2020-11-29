using SizeTree.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SizeTree.Core.Services
{
    public interface IFileService
    {
        Task<List<FileSizeInfo>> CalculateFileSizes(string rootDirPath, bool includeSubDirs);
        Task<List<FolderSizeInfo>> CalculateFolderSizes(string rootDirPath, bool includeSubDirs);
    }
}
