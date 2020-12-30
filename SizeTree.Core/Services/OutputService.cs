using SizeTree.Core.Models;
using SizeTree.Core.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SizeTree.Core.Services
{
    public class OutputService : IOutputService
    {
        public async Task WriteOutputToFile(IEnumerable<FileSizeInfo> input)
        {
            try
            {
                using StreamWriter file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @$"\output-files.txt");
                foreach (var item in input)
                {
                    await file.WriteLineAsync($"{item.FileName} ({item.PathToFile})");
                    await file.WriteLineAsync($"Size: {item.FormatedSize}");
                    await file.WriteLineAsync("------------------------------------");
                }
                await file.FlushAsync();
            }
            catch(Exception ex)
            {
                var exception = new OutputServiceException(
                    "Failure in output service",
                    new WriteOutputToFileException(
                        "Writing files size info to file has failed",
                        ex));
                throw exception;
            }
        }
        public async Task WriteOutputToFile(IEnumerable<FolderSizeInfo> input)
        {
            try
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
            catch (Exception ex)
            {
                var exception = new OutputServiceException(
                    "Failure in output service",
                    new WriteOutputToFileException(
                        "Writing folders size info to file has failed",
                        ex));
                throw exception;
            }
        }
    }
}
