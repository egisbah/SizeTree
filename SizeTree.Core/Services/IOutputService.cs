using SizeTree.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SizeTree.Core.Services
{
    public interface IOutputService
    {
        Task WriteOutputToFile(IEnumerable<FileSizeInfo> input);
        Task WriteOutputToFile(IEnumerable<FolderSizeInfo> input);
    }
}
