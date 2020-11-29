using ByteSizeLib;
using SizeTree.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SizeTree.Core.Services
{
    public class FileService : IFileService
    {
        public async Task<List<FileSizeInfo>> CalculateFileSizes(string rootDirPath, bool includeSubDirs)
        {
            var output = new List<FileSizeInfo>();
            if (!Directory.Exists(rootDirPath))
                return output;
            await Task.Run(() => 
            {
                var allFiles = includeSubDirs switch
                {
                    true => Directory.GetFiles(rootDirPath, "*.*",SearchOption.AllDirectories),
                    false => Directory.GetFiles(rootDirPath, "*.*", SearchOption.TopDirectoryOnly)
                };
                foreach (var file in allFiles)
                {
                    var fileInfo = new FileInfo(file);
                    var size = ByteSize.FromBytes(fileInfo.Length);
                    var fileSizeInfo = new FileSizeInfo();
                    fileSizeInfo.FileName = fileInfo.Name;
                    fileSizeInfo.PathToFile = fileInfo.FullName;
                    fileSizeInfo.FileSizeInBytes = size.Bytes;
                    fileSizeInfo.FileSizeInKilobytes = size.KiloBytes;
                    fileSizeInfo.FileSizeInMb = size.MegaBytes;
                    fileSizeInfo.FileSizeInGb = size.GigaBytes;
                    fileSizeInfo.FormatedSize = $"{size.LargestWholeNumberDecimalValue} {size.LargestWholeNumberDecimalSymbol}";
                    output.Add(fileSizeInfo);
                }
            });
            return output.OrderByDescending(x => x.FileSizeInBytes).ToList();
        }
        public async Task<List<FolderSizeInfo>> CalculateFolderSizes(string rootDirPath, bool includeSubDirs)
        {
            var output = new List<FolderSizeInfo>();
            if (!Directory.Exists(rootDirPath))
                return output;
            await Task.Run(() =>
            {
                var options = new EnumerationOptions();
                options.IgnoreInaccessible = true;
                var allFolders = includeSubDirs switch
                {
                    true => Directory.GetDirectories(rootDirPath, "*", new EnumerationOptions { IgnoreInaccessible = true, RecurseSubdirectories = true }),
                    false => Directory.GetDirectories(rootDirPath, "*", new EnumerationOptions { IgnoreInaccessible = true })
                };
                foreach (var folder in allFolders)
                {
                    var folderSizeInfo = new FolderSizeInfo();
                    var dir = new DirectoryInfo(folder);
                    var files = dir.EnumerateFiles("*.*", new EnumerationOptions { IgnoreInaccessible = true, RecurseSubdirectories = true });
                    var size = ByteSize.FromBytes(files.Sum(x => x.Length));
                    folderSizeInfo.FolderName = dir.Name;
                    folderSizeInfo.PathToFolder = dir.FullName;
                    folderSizeInfo.FileCount = files.Count();
                    folderSizeInfo.FolderSizeInBytes = size.Bytes;
                    folderSizeInfo.FolderSizeInMb = size.KiloBytes;
                    folderSizeInfo.FolderSizeInMb = size.MegaBytes;
                    folderSizeInfo.FolderSizeInGb = size.GigaBytes;
                    folderSizeInfo.FormatedSize = $"{size.LargestWholeNumberDecimalValue} {size.LargestWholeNumberDecimalSymbol}";
                    output.Add(folderSizeInfo);
                }
            });
            return output.OrderByDescending(x => x.FolderSizeInBytes).ToList(); ;
        }
    }
}
