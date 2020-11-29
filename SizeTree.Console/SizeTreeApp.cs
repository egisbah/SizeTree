using SizeTree.ConsoleApp.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Console = Colorful.Console;
using System.Drawing;
using System;

namespace SizeTree.ConsoleApp
{
    public interface ISizeTreeApp
    {
        Task<int> Run(string[] args, CancellationToken ct);
    }
    public class SizeTreeApp : ISizeTreeApp
    {
        private string _targetPath;
        private bool _includeSubDirs;
        private bool _writeToOutputFile;
        private int _countOfTopItems;
        private readonly IFileService _fileService;
        private readonly IOutputService _outputService;
        public SizeTreeApp(IFileService fileService, IOutputService outputService)
        {
            _fileService = fileService;
            _outputService = outputService;
        }

        public async Task<int> Run(string[] args, CancellationToken ct)
        {
            Console.WriteAscii("FILE SIZE TREE", Color.Red);
            if (args.Count() == 0)
            {
                Console.Write("Path: ");
                _targetPath = Console.ReadLine();
                Console.WriteLine("");
                Console.Write("Include sub dirs?: ");
                _includeSubDirs = bool.Parse(Console.ReadLine());
                Console.WriteLine("");
                Console.Write("Write to output file?: ");
                _writeToOutputFile = bool.Parse(Console.ReadLine());
                Console.WriteLine("");
                Console.Write("How many top files/folders to show?: ");
                _countOfTopItems = int.Parse(Console.ReadLine());
            }
            HandleArgs(args);
            var result = await _fileService.CalculateFileSizes(_targetPath, _includeSubDirs);
            if (_writeToOutputFile)
                await _outputService.WriteOutputToFile(result);
            Console.WriteLine($"Top {_countOfTopItems} biggest files:", Color.Yellow);
            result.Take(_countOfTopItems).ToList().ForEach(x =>
            {
                Console.WriteLine($"Name: {x.FileName} ({x.PathToFile})", Color.Green);
                Console.WriteLine($"Size: {x.FormatedSize}", Color.Green);
                Console.WriteLine("------------------------------------", Color.Yellow);
            });
            Console.Write("Press any key to exit...");
            Console.ReadKey();
            return 0;
        }
        private void HandleArgs(string[] args)
        {
            try
            {
                if (args.Count() == 1)
                    _targetPath = args[0];
                if (args.Count() >= 2)
                {
                    _targetPath = args[0];
                    _includeSubDirs = bool.Parse(args[1]);
                }
                if (args.Count() >= 3)
                {
                    _targetPath = args[0];
                    _includeSubDirs = bool.Parse(args[1]);
                    _writeToOutputFile = bool.Parse(args[2]);
                }
                if (args.Count() >= 4)
                {
                    _targetPath = args[0];
                    _includeSubDirs = bool.Parse(args[1]);
                    _writeToOutputFile = bool.Parse(args[2]);
                    _countOfTopItems = int.Parse(args[3]);
                }
            }
            catch
            {
                Console.WriteLine("Can't parse arguments", Color.Red);
                throw;
            }
        }
    }
}
