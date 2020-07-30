using System;
using Console.Attributes;
using Console.Commands;
using UnityEngine;

namespace Console.Console
{
	[Serializable]
	public class DefaultCommandAdder
	{
		[Tooltip("The command to display the help page")]
		public string helpCommand = "help";

		[Tooltip("The command to clear the console")]
		public string clearCommand = "clear";
		
		public void AddCommands()
		{
			CommandManager.SetHelp(helpCommand);
			AddClear();
			
			CommandAttributeUtils.AddCommands(this);
		}
		
		private void AddClear()
		{
			Command clear = new Command(clearCommand, ConsoleManager.Clear);
			clear.SetHelpMessage("Clears the console.");

			CommandManager.AddCommand(clear);
		}

		[Command("test", "Prints nothing", "t", "T", "Test")]
		private void Test()
		{
			ConsoleManager.Log("Absolutely nothing", false);
		}

		[Command("write", "Writes a message to the console", "Write")]
		private void Write(string message)
		{
			ConsoleManager.Log(message);
		}
	}
}