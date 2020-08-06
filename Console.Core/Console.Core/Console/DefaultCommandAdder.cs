using System;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Attributes.CommandSystem.Helper;
using Console.Core.Commands;
using Console.Core.Commands.CommandImplementations;

namespace Console.Core.Console
{
    [Serializable]
    public static class DefaultCommandAdder
    {
        private const string helpHelpMessage = "Displays all commands.";
        private const string help1HelpMessage = "Displays the help page of a given command.";
        private const string helpCommand = "help";
        private const string clearCommand = "clear";
        private const string clearCommandMessage = "Clears the Console";

        public static void AddDefaultCommands()
        {
            CommandAttributeUtils.AddCommands(typeof(DefaultCommandAdder));
        }

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
                AConsoleManager.Instance.Log(command.ToString());
            }
        }

        [Command(helpCommand, help1HelpMessage, "h")]
        private static void Help(string commandName)
        {
            foreach (AbstractCommand command in CommandManager.GetCommands(commandName, (bool)true))
            {
                AConsoleManager.Instance.Log(command.ToString());
            }
        }


        [Command("echo", "Echos the input")]
        private static void Echo(string value) => AConsoleManager.Instance.Log(value);
    }
}