using System;
using System.Linq;
using Console.Core.CommandSystem.Commands;



/// <summary>
/// Contains the EnumAutoFillProvider Implementations and Helper Classes
/// </summary>
namespace Console.Core.CommandSystem.Builder.EnumAutoFill
{
    /// <summary>
    /// Provides Enum Names as AutoFill Suggestions
    /// </summary>
    public class EnumAutoFillProvider : AutoFillProvider
    {
        /// <summary>
        /// Determines if the Provider can Provide Useful AutoFill Suggestions
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <param name="paramNum">Command Parameter Index</param>
        /// <returns>True if it can AutoFill</returns>
        public override bool CanFill(AbstractCommand cmd, int paramNum)
        {
            if (paramNum == 0) return false;
            if (cmd.MetaData.Count >= paramNum)
            {
                bool ret = cmd.MetaData[paramNum - 1].Attributes.Any(x => x is EnumAutoFillAttribute);
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
            if (paramNum == 0) return new string[0];
            if (cmd.MetaData.Count >= paramNum)
            {
                if (!(cmd.MetaData[paramNum - 1].Attributes.FirstOrDefault(x => x is EnumAutoFillAttribute) is EnumAutoFillAttribute ret)) return new string[0];
                Type t = ret.EnumType ?? cmd.MetaData[paramNum - 1].ParameterType;
                if (t.IsEnum)
                {
                    return Enum.GetNames(t).Where(x=>x.StartsWith(start)).ToArray();
                }
            }
            return new string[0];
        }
    }
}