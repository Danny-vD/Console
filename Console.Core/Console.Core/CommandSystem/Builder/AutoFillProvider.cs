using Console.Core.ActivationSystem;
using Console.Core.CommandSystem.Commands;

namespace Console.Core.CommandSystem.Builder
{
    /// <summary>
    /// Abstract AutoFillProvider.
    /// Is used to provide AutoFill Suggestions while typing a command
    /// </summary>
    [ActivateOn]
    public abstract class AutoFillProvider
    {

        /// <summary>
        /// Determines if the Provider can Provide Useful AutoFill Suggestions
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <param name="paramNum">Command Parameter Index</param>
        /// <returns>True if it can AutoFill</returns>
        public abstract bool CanFill(AbstractCommand cmd, int paramNum);

        /// <summary>
        /// Returns the Auto Fill Entries that are useful in the current Context.
        /// </summary>
        /// <param name="cmd">The Command</param>
        /// <param name="paramNum">The Command Parameter Index</param>
        /// <param name="start">The Start of the Parameter("search term")</param>
        /// <returns>List of Viable AutoFill Entries</returns>
        public abstract string[] GetAutoFills(AbstractCommand cmd, int paramNum, string start);

    }
}