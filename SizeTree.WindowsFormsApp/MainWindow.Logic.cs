using Microsoft.Extensions.Logging;
using SizeTree.Core.Models;
using SizeTree.WindowsFormsApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SizeTree.WindowsFormsApp
{
    public partial class MainWindow : Form
    {
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

            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, ex.InnerException);
                ShowErrorPopUp(ex.Message, ex);
                SetUiAsNotLoading();
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
    }
}
