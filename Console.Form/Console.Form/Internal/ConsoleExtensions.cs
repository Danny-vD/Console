using System.Drawing;
using System.Windows.Forms;



/// <summary>
/// Contains Internal Classes that provide helper functions or internal structures.
/// </summary>
namespace Console.Form.Internal
{

    /// <summary>
    /// Implements Colored Rich Textbox output
    /// </summary>
    internal static class ConsoleExtensions
    {
        /// <summary>
        /// Appends a Line of Text in a specified fore and backcolor
        /// </summary>
        /// <param name="box">The RichTextBox that will be written to</param>
        /// <param name="text">The Content</param>
        /// <param name="color">The Fore Color</param>
        /// <param name="backColor">The Back Color</param>
        public static void AppendLine(this RichTextBox box, string text, Color color, Color backColor)
        {
            AppendText(box, text + '\n', color, backColor);
        }



        /// <summary>
        /// Appends a Text in a specified fore and backcolor
        /// </summary>
        /// <param name="box">The RichTextBox that will be written to</param>
        /// <param name="text">The Content</param>
        /// <param name="color">The Fore Color</param>
        /// <param name="backColor">The Back Color</param>
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