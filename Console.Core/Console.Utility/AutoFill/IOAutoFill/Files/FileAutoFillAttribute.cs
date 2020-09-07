using Console.Core;

namespace Console.Utility.AutoFill.IOAutoFill.Files
{
    /// <summary>
    /// When a Parameter Gets Decorated with the FileAutoFillAttribute it enables the CommandBuilder to Suggest Possible File Entry Values for this Parameter
    /// </summary>
    public class FileAutoFillAttribute : ConsoleAttribute
    {

        private readonly string Extension;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="extension">Extension Search Term</param>
        public FileAutoFillAttribute(string extension = "*")
        {
            Extension = extension;
        }

        /// <summary>
        /// The Search Term that will be applied
        /// </summary>
        public string SearchTerm => Extension.StartsWith(".") ? Extension : '.' + Extension;

    }
}