using ByteSizeLib;
using SizeTree.Core.Models;
using SizeTree.Core.Services.Exceptions;
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
            try
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
            catch(Exception ex)
            {
                var exception = new FileServiceException(
                    "Failure in file service",
                    new CalculateFolderSizesException(
                        "Size calculation for given root path failed", 
                        ex));
                throw exception;
            }

        }
        public async IAsyncEnumerable<FolderSizeInfo> CalculateFolderSizesAsyncStream(string rootDirPath, bool includeSubDirs)
        {

            bool shouldCalculate = true;
            try
            {
                if (!Directory.Exists(rootDirPath))
                    shouldCalculate = false;
            }
            catch(Exception ex)
            {
                var exception = new FileServiceException(
                    "Failure in file service",
                    new CalculateFolderSizesAsyncStreamException(
                        "Size calculation for given root path failed",
                        ex));
                throw exception;
            }

            if (shouldCalculate)
            {
                await Task.Delay(0);
                var options = new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = includeSubDirs
                };
                string[] allFolders;
                try
                {
                    allFolders = Directory.GetDirectories(rootDirPath, "*", options);
                }
                catch (Exception ex)
                {
                    var exception = new FileServiceException(
                        "Failure in file service",
                        new CalculateFolderSizesAsyncStreamException(
                            "Size calculation for given root path failed",
                            ex));
                    throw exception;
                }
                foreach (var folder in allFolders)
                {
                    FolderSizeInfo generated = null;
                    try
                    {
                        generated = GenerateFolderInfo(folder, true);
                    }
                    catch (Exception ex)
                    {
                        var exception = new FileServiceException(
                            "Failure in file service",
                            new CalculateFolderSizesAsyncStreamException(
                                "Size calculation for given root path failed",
                                ex));
                        throw exception;
                    }
                    yield return generated;
                }
                FolderSizeInfo root = null;
                try
                {
                    root = GenerateFolderInfo(rootDirPath, false);
                }
                catch (Exception ex)
                {
                    var exception = new FileServiceException(
                        "Failure in file service",
                        new CalculateFolderSizesAsyncStreamException(
                            "Size calculation for given root path failed",
                            ex));
                    throw exception;
                }
                yield return root;
            }

        }
        public async Task<int> GetCountOfSubDirectories(string rootDirPath)
        {
            try
            {
                await Task.Delay(0);
                var options = new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true
                };
                var allFolders = Directory.GetDirectories(rootDirPath, "*", options);
                return allFolders.Count();
            }
            catch(Exception ex)
            {
                var exception = new FileServiceException(
                    "Failure in file service",
                    new CalculateFolderSizesAsyncStreamException(
                        "Getting count of subdirectories failed",
                        ex));
                throw exception;
            }
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
