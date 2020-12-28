using SizeTree.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SizeTree.Core.Services
{
    public interface IOutputService
    {
        Task WriteOutputToFile(List<FileSizeInfo> input);
        Task WriteOutputToFile(List<FolderSizeInfo> input);
    }
}
