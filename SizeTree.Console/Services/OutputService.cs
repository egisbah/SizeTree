using SizeTree.ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeTree.ConsoleApp.Services
{
    public class OutputService : IOutputService
    {
        public async Task WriteOutputToFile(List<FileSizeInfo> input)
        {
            using StreamWriter file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"\output.txt");
            foreach(var item in input)
            {
                await file.WriteLineAsync($"{item.FileName} ({item.PathToFile})");
                await file.WriteLineAsync($"Size: {item.FormatedSize}");
                await file.WriteLineAsync("------------------------------------");
            }
            await file.FlushAsync();
        }
    }
}
