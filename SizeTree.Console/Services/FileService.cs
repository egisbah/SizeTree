using SizeTree.ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteSizeLib;

namespace SizeTree.ConsoleApp.Services
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
                    true => Directory.GetFiles(rootDirPath, "*.*", SearchOption.AllDirectories),
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
    }
}
