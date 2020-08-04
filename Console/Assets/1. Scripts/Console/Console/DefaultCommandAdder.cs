using System;
using Console.Attributes.CommandSystem;
using Console.Attributes.CommandSystem.Helper;
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

			CommandAttributeUtils.AddCommands<DefaultCommandAdder>();
		}

		private void AddClear()
		{
			Command clear = new Command(clearCommand, ConsoleManager.Clear);
			clear.SetHelpMessage("Clears the console.");

			CommandManager.AddCommand(clear);
		}

		[Command("exit", "Closes the application.", "Exit", "Quit", "quit")]
		private static void Exit()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#else
			Application.Quit();
#endif
		}
	}
}