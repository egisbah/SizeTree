using ByteSizeLib;
using SizeTree.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SizeTree.Core.Services
{
    public class FileService : IFileService
    {
        public async Task<List<FolderSizeInfo>> CalculateFolderSizes(string rootDirPath, bool includeSubDirs)
        {
            var calculatedFolderSizes = new List<FolderSizeInfo>();
            if (!Directory.Exists(rootDirPath))
                return calculatedFolderSizes;
            await Task.Run(() =>
            {
                var options = new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = includeSubDirs
                };
                var allFolders = Directory.GetDirectories(rootDirPath, "*", options);
                foreach (var folder in allFolders)
                {
                    var generated = GenerateFolderInfo(folder, true);
                    calculatedFolderSizes.Add(generated);
                }

                calculatedFolderSizes.Add(GenerateFolderInfo(rootDirPath, false));
            });
            var output = calculatedFolderSizes.OrderByDescending(x => x.FolderSizeInBytes).ToList();

            return output;
        }
        private FolderSizeInfo GenerateFolderInfo(string path, bool includeSubDirs, IEnumerable<FileInfo> additionalFiles = null)
        {
            var folderSizeInfo = new FolderSizeInfo();
            var dir = new DirectoryInfo(path);
            var files = dir.EnumerateFiles("*.*", new EnumerationOptions { IgnoreInaccessible = true, RecurseSubdirectories = includeSubDirs }).ToList();
            if (additionalFiles != null)
                files.AddRange(additionalFiles);
            var size = ByteSize.FromBytes(files.Sum(x => x.Length));
            folderSizeInfo.FolderName = dir.Name;
            folderSizeInfo.PathToFolder = dir.FullName;
            folderSizeInfo.FileCount = files.Count();
            folderSizeInfo.FolderSizeInBytes = size.Bytes;
            folderSizeInfo.FolderSizeInMb = size.KiloBytes;
            folderSizeInfo.FolderSizeInMb = size.MegaBytes;
            folderSizeInfo.FolderSizeInGb = size.GigaBytes;
            folderSizeInfo.FormatedSize = $"{size.LargestWholeNumberDecimalValue} {size.LargestWholeNumberDecimalSymbol}";
            folderSizeInfo.Files = GenerateFileSizeInfo(files);
            return folderSizeInfo;
        }
        private List<FileSizeInfo> GenerateFileSizeInfo(List<FileInfo> input)
        {
            var generatedFileSizes = new List<FileSizeInfo>();

            foreach (var file in input)
            {
                var size = ByteSize.FromBytes(file.Length);
                var fileSizeInfo = new FileSizeInfo();
                fileSizeInfo.FileName = file.Name;
                fileSizeInfo.PathToFile = file.FullName;
                fileSizeInfo.FileSizeInBytes = size.Bytes;
                fileSizeInfo.FileSizeInKilobytes = size.KiloBytes;
                fileSizeInfo.FileSizeInMb = size.MegaBytes;
                fileSizeInfo.FileSizeInGb = size.GigaBytes;
                fileSizeInfo.FormatedSize = $"{size.LargestWholeNumberDecimalValue} {size.LargestWholeNumberDecimalSymbol}";
                generatedFileSizes.Add(fileSizeInfo);
            }

            var output = generatedFileSizes.OrderByDescending(x => x.FileSizeInBytes).ToList();

            return output;
        }
    }
}
