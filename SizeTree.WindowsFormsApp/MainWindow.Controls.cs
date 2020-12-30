using System;
using System.Windows.Forms;

namespace SizeTree.WindowsFormsApp
{
    public partial class MainWindow : Form
    {
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
        private void ShowErrorPopUp(string message)
        {
            MessageBox.Show(message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void ShowErrorPopUp(string message, Exception ex)
        {
            var text = $"{message}, {ex?.Message}, {ex?.InnerException?.Message}";
            MessageBox.Show(text, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
