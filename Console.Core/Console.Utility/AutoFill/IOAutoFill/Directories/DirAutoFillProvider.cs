using System.IO;
using System.Linq;

using Console.Core.CommandSystem.Builder;
using Console.Core.CommandSystem.Commands;


/// <summary>
/// Contains the DirAutoFillProvider Implementations and Helper Classes
/// </summary>
namespace Console.Utility.AutoFill.IOAutoFill.Directories
{
    /// <summary>
    /// Provides Directory Paths as AutoFill Suggestions
    /// </summary>
    public class DirAutoFillProvider : AutoFillProvider
    {

        /// <summary>
        /// Determines if the Provider can Provide Useful AutoFill Suggestions
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <param name="paramNum">Command Parameter Index</param>
        /// <returns>True if it can AutoFill</returns>
        public override bool CanFill(AbstractCommand cmd, int paramNum)
        {
            if (paramNum == 0)
            {
                return false;
            }

            if (cmd.MetaData.Count >= paramNum)
            {
                bool ret = cmd.MetaData[paramNum - 1].Attributes.Any(x => x is DirAutoFillAttribute);
                return ret;
            }

            return false;
        }

        /// <summary>
        /// Returns the Auto Fill Entries that are useful in the current Context.
        /// </summary>
        /// <param name="cmd">The Command</param>
        /// <param name="paramNum">The Command Parameter Index</param>
        /// <param name="start">The Start of the Parameter("search term")</param>
        /// <returns>List of Viable AutoFill Entries</returns>
        public override string[] GetAutoFills(AbstractCommand cmd, int paramNum, string start)
        {
            string s = string.IsNullOrEmpty(start) ? ".\\" : start.StartsWith("./") ? start : "./" + start;
            string[] entries = Directory.GetFiles(Path.GetDirectoryName(s), "*", SearchOption.TopDirectoryOnly);
            string strt = s.Replace("/", "\\");
            return entries.Where(x => x.StartsWith(strt)).ToArray();
        }

    }
}