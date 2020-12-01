using System.Collections.Generic;

namespace SizeTree.Core.Models
{
    public class FolderSizeInfo
    {
        public string FolderName { get; set; }
        public string PathToFolder { get; set; }
        public int FileCount { get; set; }
        public double FolderSizeInBytes { get; set; }
        public double FolderSizeInKilobytes { get; set; }
        public double FolderSizeInMb { get; set; }
        public double FolderSizeInGb { get; set; }
        public string FormatedSize { get; set; }
        public List<FileSizeInfo> Files { get; set; }
    }
}
