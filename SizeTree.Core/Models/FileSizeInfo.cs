namespace SizeTree.Core.Models
{
    public class FileSizeInfo
    {
        public string FileName { get; set; }
        public string PathToFile { get; set; }
        public double FileSizeInBytes { get; set; }
        public double FileSizeInKilobytes { get; set; }
        public double FileSizeInMb { get; set; }
        public double FileSizeInGb { get; set; }
        public string FormatedSize { get; set; }
    }
}
