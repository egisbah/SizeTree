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
        public MainWindow(IFileService fileService, IOutputService outputService)
        {
            _fileService = fileService;
            _outputService = outputService;
            InitializeComponent();
            SetUiAsNotLoading();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var folderCount = 0;
            if (subDirCheckBox.Checked)
            {
                await Task.Run(async () =>
                {
                    folderCount = await _fileService.GetCountOfSubDirectories(textBox1.Text);
                });
                SetDynamicProgressBar(folderCount);
            }
            else
                SetStaticProgressBar();

            SetUiAsLoading();

            await Task.Run(async () =>
            {
                if (subDirCheckBox.Checked)
                {
                    Folders = new List<FolderSizeInfo>();
                    await foreach (var item in _fileService.CalculateFolderSizesAsyncStream(textBox1.Text, subDirCheckBox.Checked))
                    {
                        Folders.Add(item);
                        PushProgressBar();
                    }
                }
                else
                    Folders = await _fileService.CalculateFolderSizes(textBox1.Text, subDirCheckBox.Checked);
                
                Files = Folders.SelectMany(x => x.Files).ToList();

                if (this.writeToFileCheckBox.Checked)
                {
                    await _outputService.WriteOutputToFile(Files);
                    await _outputService.WriteOutputToFile(Folders);
                }
                
            });

            richTextBox1.Text = "";
            Folders.OrderByDescending(x => x.FolderSizeInBytes)
                    .Take(int.Parse(comboBox1.SelectedItem.ToString()))
                    .ToList()
                    .ForEach(x =>
            {
                richTextBox1.AppendText($"Name: {x.FolderName} ({x.PathToFolder})");
                richTextBox1.AppendText(Environment.NewLine);
                richTextBox1.AppendText($"Size: {x.FormatedSize}");
                richTextBox1.AppendText(Environment.NewLine);
                richTextBox1.AppendText($"File count: {x.FileCount}");
                richTextBox1.AppendText(Environment.NewLine);
                richTextBox1.AppendText("------------------------------------");
                richTextBox1.AppendText(Environment.NewLine);
                richTextBox1.GoToLine(0);
            });
            richTextBox2.Text = "";
            Files.Take(int.Parse(comboBox1.SelectedItem.ToString())).ToList().ForEach(x =>
            {
                richTextBox2.AppendText($"Name: {x.FileName} ({x.PathToFile})");
                richTextBox2.AppendText(Environment.NewLine);
                richTextBox2.AppendText($"Size: {x.FormatedSize}");
                richTextBox2.AppendText(Environment.NewLine);
                richTextBox2.AppendText("------------------------------------");
                richTextBox2.AppendText(Environment.NewLine);
                richTextBox1.GoToLine(0);
            });
            SetUiAsNotLoading();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;
        }
        private void PushProgressBar()
        {
            // This is written in explicit way to be able to interact with from from Task
            this.progressBar1.BeginInvoke(new Action(() =>
            {
                this.progressBar1.PerformStep();
            }));
        }
        private void SetDynamicProgressBar(int count)
        {
            progressBar1.Maximum = count;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            progressBar1.Style = ProgressBarStyle.Blocks;
        }
        private void SetStaticProgressBar()
        {
            progressBar1.Style = ProgressBarStyle.Marquee;
        }
        private void SetUiAsNotLoading()
        {
            this.progressBar1.Visible = false;
            this.button1.Enabled = true;
            this.button2.Enabled = true;
            this.subDirCheckBox.Enabled = true;
            this.writeToFileCheckBox.Enabled = true;
            this.textBox1.Enabled = true;
            this.comboBox1.Enabled = true;
        }
        private void SetUiAsLoading()
        {
            this.progressBar1.Visible = true;
            this.button1.Enabled = false;
            this.button2.Enabled = false;
            this.subDirCheckBox.Enabled = false;
            this.writeToFileCheckBox.Enabled = false;
            this.textBox1.Enabled = false;
            this.comboBox1.Enabled = false;
        }
    }
}
