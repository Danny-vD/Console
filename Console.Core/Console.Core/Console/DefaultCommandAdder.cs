using System;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Attributes.CommandSystem.Helper;
using Console.Core.Commands;
using Console.Core.Commands.CommandImplementations;

namespace Console.Core.Console
{
	[Serializable]
	public class DefaultCommandAdder
	{
		public string helpCommand = "help";
        
		public string clearCommand = "clear";

        public void AddCommands()
		{
			CommandManager.SetHelp(helpCommand);
			AddClear();

			CommandAttributeUtils.AddCommands<DefaultCommandAdder>();
		}

		private void AddClear()
		{
			Command clear = new Command(clearCommand, AConsoleManager.Instance.Clear);
			clear.SetHelpMessage("Clears the console.");

			CommandManager.AddCommand(clear);
		}

        [Command("echo", "Echos the input")]
        private static void Echo(string value) => AConsoleManager.Instance.Log(value);

		[Command("exit", "Closes the application.", "Exit", "Quit", "quit")]
		private static void Exit()
		{
            //TODO
		}
	}
}