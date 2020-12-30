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

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SetUiAsLoading();
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

            catch(Exception ex)
            {
                SetUiAsNotLoading();
                _logger.LogError(ex.Message, ex);
            }
            finally
            {
                Folders = null;
                Files = null;
            }
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
                this.label5.Text = $"{this.progressBar1.Value} / {this.progressBar1.Maximum}";
            }));
        }
        private void SetDynamicProgressBar(int count)
        {
            progressBar1.Maximum = count;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            progressBar1.Style = ProgressBarStyle.Blocks;

            label5.Text = $"0 / {count}";
        }
        private void SetStaticProgressBar()
        {
            progressBar1.Style = ProgressBarStyle.Marquee;
        }
        private void InitialUiSetup()
        {
            this.toolTip1.SetToolTip(this.writeToFileCheckBox, "Writes full lists of files & folders to applications root directory");
            this.toolTip2.SetToolTip(this.subDirCheckBox, "Check if you want to include full folder tree under specified directory");
            this.toolTip3.SetToolTip(this.comboBox1, "Choose how many records will be shown in files & folders output windows");
            this.comboBox1.SelectedItem = 10;
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
            this.label5.Visible = false;
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
            this.label5.Visible = true;
        }
    }
}
