using SizeTree.Core.Models;
using SizeTree.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SizeTree.WindowsFormsApp
{
    public partial class MainWindow : Form
    {
        private readonly IFileService _fileService;
        private readonly IOutputService _outputService;

        private bool IsLoading = false;
        public MainWindow(IFileService fileService, IOutputService outputService)
        {
            _fileService = fileService;
            _outputService = outputService;
            InitializeComponent();
            SetUiAsNotLoading();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SetUiAsLoading();
            var folders = await Wtf(this.textBox1.Text, this.subDirCheckBox.Checked);
            SetUiAsNotLoading();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;
        }

        private async Task<List<FolderSizeInfo>> Wtf(string path, bool includeSubDirs)
        {
            await Task.Delay(1);
            return await _fileService.CalculateFolderSizes(path, includeSubDirs);
        }

        private void SetUiAsNotLoading()
        {
            this.progressBar1.Visible = false;
            this.button1.Enabled = true;
            this.button2.Enabled = true;
            this.subDirCheckBox.Enabled = true;
            this.writeToFileCheckBox.Enabled = true;
            this.textBox1.Enabled = true;
        }
        private void SetUiAsLoading()
        {
            this.progressBar1.Visible = true;
            this.button1.Enabled = false;
            this.button2.Enabled = false;
            this.subDirCheckBox.Enabled = false;
            this.writeToFileCheckBox.Enabled = false;
            this.textBox1.Enabled = false;
        }
    }
}
