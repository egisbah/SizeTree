using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SizeTree.WindowsFormsApp
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
