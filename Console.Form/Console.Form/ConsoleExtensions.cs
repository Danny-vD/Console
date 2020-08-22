using System.Drawing;
using System.Windows.Forms;

namespace Console.Form
{
    internal static class ConsoleExtensions
    {
        public static void AppendLine(this RichTextBox box, string text, Color color, Color backColor)
        {
            AppendText(box, text + '\n', color, backColor);
        }

        public static void AppendText(this RichTextBox box, string text, Color color, Color backColor)
        {
            float zoom = box.ZoomFactor;
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.SelectionBackColor = backColor;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
            box.ZoomFactor = zoom;
        }
    }
}