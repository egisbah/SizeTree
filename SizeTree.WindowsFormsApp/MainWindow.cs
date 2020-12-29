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
            try
            {
                SetUiAsLoading();
                await Task.Run(async () =>
                {
                    Folders = await _fileService.CalculateFolderSizes(textBox1.Text, subDirCheckBox.Checked);
                    Files = Folders.SelectMany(x => x.Files).ToList();

                    if (this.writeToFileCheckBox.Checked)
                    {
                        await _outputService.WriteOutputToFile(Files);
                        await _outputService.WriteOutputToFile(Folders);
                    }
                });

                richTextBox1.Text = "";
                Folders.Take(int.Parse(comboBox1.SelectedItem.ToString())).ToList().ForEach(x =>
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
                throw;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;
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
