using Microsoft.Extensions.Logging;
using SizeTree.Core.Models;
using SizeTree.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SizeTree.WindowsFormsApp
{
    public partial class MainWindow : Form
    {
        private readonly IFileService _fileService;
        private readonly IOutputService _outputService;
        private List<FolderSizeInfo> Folders;
        private List<FileSizeInfo> Files;
        private readonly ILogger<MainWindow> _logger;
        public MainWindow(IFileService fileService, IOutputService outputService, ILogger<MainWindow> logger)
        {
            _fileService = fileService;
            _outputService = outputService;
            _logger = logger;
            InitializeComponent();
            InitialUiSetup();
            SetUiAsNotLoading();
        }
    }
}
