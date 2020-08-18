
/// <summary>
/// The Console.Core.CommandSystem.Commands.BuiltIn namespace contains all BuiltIn Commands of the System
/// </summary>

namespace Console.Core.CommandSystem.Commands.BuiltIn
{

    public class FlagTests
    {
        public static void AddFlagTestCommands()
        {
            CommandAttributeUtils.AddCommands<FlagTests>();
        }

        [Command("ft", "Flag Test. This Command is Legal")]
        private static void FlagTest(string arg0, int arg1, [CommandFlag("x")] bool flag1, [CommandFlag("y")]bool flag2) //(2-4)
        {
            AConsoleManager.Instance.Log("(string)arg0: " + arg0 + "\n(int)arg1: " + arg1 + "\nx: " + flag1 + "\ny: " + flag2);
        }
        [Command("ft", "Flag Test. This Command is Illegal)")]
        private static void FlagTest(string arg0, int arg1, [CommandFlag("x")] bool flag1) //Illegal (2-3)
        {
            AConsoleManager.Instance.Log("(string)arg0: " + arg0 + "\n(int)arg1: " + arg1 + "\nx: " + flag1);
        }

        [Command("ft", "Flag Test. This Command is Legal")]
        private static void FlagTest(string arg0, [CommandFlag("x")] bool flag1, [CommandFlag("y")]bool flag2) //Legal(1-3)
        {
            AConsoleManager.Instance.Log("(string)arg0: " + arg0 + "\nx: " + flag1 + "\ny: " + flag2);
        }
        [Command("ft", "Flag Test. This Command is Illegal")]
        private static void FlagTest(int arg0, [CommandFlag("x")] bool flag1, [CommandFlag("y")]bool flag2) //Illegal(1-3)
        {
            AConsoleManager.Instance.Log("(int)arg0: " + arg0 + "\nx: " + flag1 + "\ny: " + flag2);
        }
    }

    /// <summary>
    /// Console Default Commands like clear/help/echo
    /// </summary>
    public class DefaultCommands
    {

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
                AConsoleManager.Instance.Log(command.ToString(shortInfo ? ToStringMode.Short : ToStringMode.Long) + "\n--------------------------");
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
                AConsoleManager.Instance.Log(command.ToString(shortInfo ? ToStringMode.Short : ToStringMode.Long) + "\n--------------------------");
            }
        }

        /// <summary>
        /// Echo Command.
        /// </summary>[Flag
        /// <param name="value">Value to Write to the Console.</param>
        [Command("echo", "Echos the input")]
        private static void Echo(string value) => AConsoleManager.Instance.Log(value);

        #endregion
    }
}