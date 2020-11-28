using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeTree.ConsoleApp.Models
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
