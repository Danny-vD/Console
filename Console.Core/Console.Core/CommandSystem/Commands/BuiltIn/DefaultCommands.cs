using Console.Core.CommandSystem.Builder.CommandAutoFill;
using Console.Core.ILOptimizations;
using Console.Core.LogSystem;


/// <summary>
/// The Console.Core.CommandSystem.Commands.BuiltIn namespace contains all BuiltIn Commands of the System
/// </summary>
namespace Console.Core.CommandSystem.Commands.BuiltIn
{
    /// <summary>
    /// Console Default Commands like clear/help/echo
    /// </summary>
    public class DefaultCommands
    {
        private static readonly ALogger EchoLogger = TypedLogger.CreateTypedWithPrefix("echo");
        private static readonly ALogger HelpLogger = TypedLogger.CreateTypedWithPrefix("help");

        #region Internal Data

        private const string helpHelpMessage = "Displays all Commands. -s for shorter version.";
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
        [OptimizeIL]
        public static void Clear()
        {
            AConsoleManager.Instance.Clear();
        }


        /// <summary>
        /// Help Command.
        /// <param name="shortInfo">Flag -s that optionally returns a shortened version of the Commands.</param>
        /// </summary>
        [Command(helpCommand, helpHelpMessage, "h")]
        [OptimizeIL]
        public static void Help([CommandFlag("s")] bool shortInfo)
        {
            foreach (AbstractCommand command in CommandManager.Commands)
            {
                HelpLogger.Log(command.ToString(shortInfo ? ToStringMode.Short : ToStringMode.Long) +
                               "\n--------------------------");
            }
        }

        /// <summary>
        /// Help Command
        /// </summary>
        /// <param name="commandName">Search Term</param>
        /// <param name="shortInfo">Flag -s that optionally returns a shortened version of the Commands.</param>
        [Command(helpCommand, help1HelpMessage, "h")]
        [OptimizeIL]
        public static void Help([CommandAutoFill] string commandName, [CommandFlag("s")] bool shortInfo)
        {
            foreach (AbstractCommand command in CommandManager.GetCommands(commandName, true))
            {
                HelpLogger.Log(command.ToString(shortInfo ? ToStringMode.Short : ToStringMode.Long) +
                               "\n--------------------------");
            }
        }

        /// <summary>
        /// Echo Command.
        /// </summary>
        /// <param name="value">Value to Write to the Console.</param>
        [Command("echo", "Echos the input")]
        [OptimizeIL]
        public static void Echo(string value)
        {
            EchoLogger.Log(value);
        }

        #endregion
    }
}