using System.Drawing;
using System.Windows.Forms;

namespace Console.Form.Internal
{
    /// <summary>
    /// Internal Representation of a Log that has been written to the Console.
    /// </summary>
    internal class ConsoleLine
    {
        /// <summary>
        /// The Content of the Log
        /// </summary>
        public readonly string Content;
        /// <summary>
        /// The Fore Color
        /// </summary>
        public readonly Color TextColor;
        /// <summary>
        /// The Back Color
        /// </summary>
        public readonly Color BackColor;
        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="content">The Log Content</param>
        /// <param name="textColor">The Fore Color</param>
        /// <param name="backColor">The Back Color</param>
        public ConsoleLine(string content, Color textColor, Color backColor)
        {
            Content = content;
            TextColor = textColor;
            BackColor = backColor;
        }

        /// <summary>
        /// Writes the Log to the specified RichTextBox.
        /// </summary>
        /// <param name="rtb">RichTextBox to write to.</param>
        public void WriteToOut(RichTextBox rtb)
        {
            rtb.AppendLine(Content, TextColor, BackColor);
        }
    }
}