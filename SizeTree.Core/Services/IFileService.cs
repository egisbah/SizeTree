﻿using SizeTree.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SizeTree.Core.Services
{
    public interface IFileService
    {
        Task<List<FolderSizeInfo>> CalculateFolderSizes(string rootDirPath, bool includeSubDirs);
        IAsyncEnumerable<FolderSizeInfo> CalculateFolderSizesAsyncStream(string rootDirPath, bool includeSubDirs);
        Task<int> GetCountOfSubDirectories(string rootDirPath);
    }
}
