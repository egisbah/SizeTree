using System.Windows.Forms;

namespace SizeTree.WindowsFormsApp.Helpers
{
    internal static class FormsExtensions
    {
        internal static void GoToLine(this RichTextBox rtb, int lineNumber)
        {
            int index = rtb.GetFirstCharIndexFromLine(lineNumber);
            rtb.Select(index, 0);
            rtb.ScrollToCaret();
        }
    }
}
