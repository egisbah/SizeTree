using SizeTree.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SizeTree.Core.Services
{
    public class OutputService : IOutputService
    {
        public async Task WriteOutputToFile(List<FileSizeInfo> input)
        {
            using StreamWriter file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @$"\output-files.txt");
            foreach(var item in input)
            {
                await file.WriteLineAsync($"{item.FileName} ({item.PathToFile})");
                await file.WriteLineAsync($"Size: {item.FormatedSize}");
                await file.WriteLineAsync("------------------------------------");
            }
            await file.FlushAsync();
        }
        public async Task WriteOutputToFile(List<FolderSizeInfo> input)
        {
            using StreamWriter file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @$"\output-folders.txt");
            foreach (var item in input)
            {
                await file.WriteLineAsync($"{item.FolderName} ({item.PathToFolder})");
                await file.WriteLineAsync($"Size: {item.FormatedSize}");
                await file.WriteLineAsync($"File count: {item.FileCount}");
                await file.WriteLineAsync("------------------------------------");
            }
            await file.FlushAsync();
        }
    }
}
