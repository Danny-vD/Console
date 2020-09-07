using System;
using System.Linq;

using Console.Core.CommandSystem.Commands;


/// <summary>
/// Contains the CommandAutoFillProvider Implementations and Helper Classes
/// </summary>
namespace Console.Core.CommandSystem.Builder.BuiltIn.CommandAutoFill
{
    /// <summary>
    /// Provides Command Names as AutoFill Suggestions
    /// </summary>
    public class CommandAutoFillProvider : AutoFillProvider
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
                return true;
            }

            if (cmd.MetaData.Count >= paramNum)
            {
                foreach (Attribute attribute in cmd.MetaData[paramNum - 1].Attributes)
                {
                    if (attribute is CommandAutoFillAttribute)
                    {
                        return true;
                    }
                }

                return false;
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
            return CommandManager.AllCommands.Where(x => x.Identity.FilteredContainsName(start, true))
                                 .Select(x => x.Identity.QualifiedName).ToArray();
        }

    }
}