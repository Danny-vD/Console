using System.Linq;
using Console.Core.CommandSystem.Commands;


/// <summary>
/// Contains the BoolAutoFillProvider Implementations and Helper Classes
/// </summary>
namespace Console.Core.CommandSystem.Builder.BoolAutoFill
{

    /// <summary>
    /// Provides Boolean Values as AutoFill Suggestions
    /// </summary>
    public class BoolAutoFillProvider : AutoFillProvider
    {
        /// <summary>
        /// Determines if the Provider can Provide Useful AutoFill Suggestions
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <param name="paramNum">Command Parameter Index</param>
        /// <returns>True if it can AutoFill</returns>
        public override bool CanFill(AbstractCommand cmd, int paramNum)
        {
            if (paramNum == 0) //The Command itself
                return false;
            if (cmd.MetaData.Count >= paramNum) //Check metadata (parameter) count
                return cmd.MetaData[paramNum - 1].ParameterType == typeof(bool) && !cmd.MetaData[paramNum-1].Attributes.Any(x=> x is CommandFlagAttribute); //Use offset by one because of command name
            return false; //Default Case
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
            return new[] {"True", "False"};
        }
    }
}