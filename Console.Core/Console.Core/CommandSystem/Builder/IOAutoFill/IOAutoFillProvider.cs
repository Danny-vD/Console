using System.IO;
using System.Linq;
using Console.Core.CommandSystem.Commands;

namespace Console.Core.CommandSystem.Builder.IOAutoFill
{
    /// <summary>
    /// 
    /// </summary>
    public class IOAutoFillProvider : AutoFillProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramNum"></param>
        /// <returns></returns>
        public override bool CanFill(CommandBuilder builder, AbstractCommand cmd, int paramNum)
        {
            if (paramNum == 0) return false;
            if (cmd.MetaData.Count >= paramNum)
            {
                bool ret = cmd.MetaData[paramNum - 1].Attributes.Any(x => x is IOAutoFillAttribute);
                return ret;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public override string[] GetAutoFills(AbstractCommand cmd, int paramNum, string start)
        {
            string[] entries = Directory.GetFileSystemEntries(string.IsNullOrEmpty(start) ? ".\\" : Path.GetDirectoryName(start), "*", SearchOption.TopDirectoryOnly);
            string strt = start.Replace("/", "\\");
            return entries.Where(x => x.StartsWith(strt)).ToArray();
        }
    }
}