﻿namespace Console.Core.CommandSystem.Commands.BuiltIn
{

    /// <summary>
    /// Console Default Commands
    /// </summary>
    public class DefaultCommands
    {

        #region Internal Data

        private const string helpHelpMessage = "Displays all commands.";
        private const string help1HelpMessage = "Displays the help page of a given command.";
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
        /// </summary>
        [Command(helpCommand, helpHelpMessage, "h")]
        private static void Help()
        {
            foreach (AbstractCommand command in CommandManager.commands)
            {
                AConsoleManager.Instance.Log(command + "\n--------------------------");
            }
        }

        /// <summary>
        /// Help Command
        /// </summary>
        /// <param name="commandName">Search Term</param>
        [Command(helpCommand, help1HelpMessage, "h")]
        private static void Help(string commandName)
        {
            foreach (AbstractCommand command in CommandManager.GetCommands(commandName, (bool)true))
            {
                AConsoleManager.Instance.Log(command+"\n--------------------------");
            }
        }


        /// <summary>
        /// Echo Command.
        /// </summary>
        /// <param name="value">Value to Write to the Console.</param>
        [Command("echo", "Echos the input")]
        private static void Echo(string value) => AConsoleManager.Instance.Log(value);

        #endregion
}
}