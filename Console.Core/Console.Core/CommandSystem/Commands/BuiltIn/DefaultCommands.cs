
/// <summary>
/// The Console.Core.CommandSystem.Commands.BuiltIn namespace contains all BuiltIn Commands of the System
/// </summary>

using Console.Core.LogSystem;

namespace Console.Core.CommandSystem.Commands.BuiltIn
{
    /// <summary>
    /// Console Default Commands like clear/help/echo
    /// </summary>
    public class DefaultCommands
    {
        private static PrefixLogger EchoLogger = new PrefixLogger("echo");
        private static PrefixLogger HelpLogger = new PrefixLogger("help");

        #region Internal Data

        private const string helpHelpMessage = "Displays all commands. -s for shorter version.";
        private const string help1HelpMessage = "Displays the help page of a given command. -s for shorter version.";
        private const string helpCommand = "help";
        private const string clearCommand = "clear";
        private const string clearCommandMessage = "Clears the Console";

        #endregion

        /// <summary>
        /// Adds all Default Commands.
        /// </summary>
        public static void AddDefaultCommands()
        {
            CommandAttributeUtils.AddCommands<DefaultCommands>();
        }

        #region Default Commands

        /// <summary>
        /// Clear Command
        /// </summary>
        [Command(clearCommand, clearCommandMessage, "cls", "clr")]
        private static void Clear()
        {
            AConsoleManager.Instance.Clear();
        }


        /// <summary>
        /// Help Command.
        /// <param name="shortInfo">Flag -s that optionally returns a shortened version of the commands.</param>
        /// </summary>
        [Command(helpCommand, helpHelpMessage, "h")]
        private static void Help([CommandFlag("s")] bool shortInfo)
        {
            foreach (AbstractCommand command in CommandManager.commands)
            {
                HelpLogger.Log(command.ToString(shortInfo ? ToStringMode.Short : ToStringMode.Long) + "\n--------------------------");
            }
        }

        /// <summary>
        /// Help Command
        /// </summary>
        /// <param name="commandName">Search Term</param>
        /// <param name="shortInfo">Flag -s that optionally returns a shortened version of the commands.</param>
        [Command(helpCommand, help1HelpMessage, "h")]
        private static void Help(string commandName, [CommandFlag("s")] bool shortInfo)
        {
            foreach (AbstractCommand command in CommandManager.GetCommands(commandName, true))
            {
                HelpLogger.Log(command.ToString(shortInfo ? ToStringMode.Short : ToStringMode.Long) + "\n--------------------------");
            }
        }

        /// <summary>
        /// Echo Command.
        /// </summary>
        /// <param name="value">Value to Write to the Console.</param>
        [Command("echo", "Echos the input")]
        private static void Echo(string value) => EchoLogger.Log(value);

        #endregion
    }
}