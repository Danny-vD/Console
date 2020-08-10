using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;
using Console.Core.Utils;

namespace Console.Core.Commands.BuiltIn
{
    public class DefaultCommands
    {

        #region Internal Data

        private const string helpHelpMessage = "Displays all commands.";
        private const string help1HelpMessage = "Displays the help page of a given command.";
        private const string helpCommand = "help";
        private const string clearCommand = "clear";
        private const string clearCommandMessage = "Clears the Console";

        #endregion

        public static void AddDefaultCommands()
        {
            CommandAttributeUtils.AddCommands<DefaultCommands>();
        }

        #region Default Commands

        [Command(clearCommand, clearCommandMessage, "cls", "clr")]
        private static void Clear()
        {
            AConsoleManager.Instance.Clear();
        }


        [Command(helpCommand, helpHelpMessage, "h")]
        private static void Help()
        {
            foreach (AbstractCommand command in CommandManager.commands)
            {
                AConsoleManager.Instance.Log(command + "\n--------------------------");
            }
        }

        [Command(helpCommand, help1HelpMessage, "h")]
        private static void Help(string commandName)
        {
            foreach (AbstractCommand command in CommandManager.GetCommands(commandName, (bool)true))
            {
                AConsoleManager.Instance.Log(command+"\n--------------------------");
            }
        }


        [Command("echo", "Echos the input")]
        private static void Echo(string value) => AConsoleManager.Instance.Log(value);

        #endregion
}
}