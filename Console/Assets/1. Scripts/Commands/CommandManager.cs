using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console;

namespace Commands
{
	public static class CommandManager
	{
		private const string helpHelpMessage = "Displays all commands.";
		private const string help1HelpMessage = "Displays the help page of a given command.";

		private static readonly List<AbstractCommand> commands = new List<AbstractCommand>();

		public static void Invoke(string commandName, params object[] parameters)
		{
			GetCommand(commandName, parameters.Length).Invoke(parameters);
		}
		
		public static void RenameCommand(string commandName, int paramsCount, string newName)
		{
			AbstractCommand command = GetCommand(commandName, paramsCount);

			if (commands.Any(item => item.HasName(newName)))
			{
				// log to the console that newName already exists
				ConsoleManager.Instance.LogError($"Rename failed! A command with name {newName} already exists!");
				return;
			}

			command.SetName(newName);
		}

		public static AbstractCommand GetCommand(string commandName, int paramsCount)
		{
			AbstractCommand command = commands.FirstOrDefault(item => item.HasName(commandName) &&
																	  item.ParametersCount == paramsCount);

			if (command == null)
			{
				ConsoleManager.Instance.LogError($"A command with name {commandName} and {paramsCount} parameter(s) does not exist!");
			}

			return command;
		}

		public static List<AbstractCommand> GetCommands(string commandName)
		{
			List<AbstractCommand> tempCommands = commands.Where(command => command.HasName(commandName)).ToList();

			if (tempCommands.Count == 0)
			{
				ConsoleManager.Instance.LogError($"A command with name {commandName} does not exist!");
			}

			return tempCommands;
		}

		public static void AddCommand(AbstractCommand command)
		{
			if (commands.Any(item => item.HasName(command.GetAllNames()) &&
									 item.ParametersCount == command.ParametersCount))
			{
				ConsoleManager.Instance.LogError(
					$"A command with name {ToString(command.GetAllNames())} with {command.ParametersCount} parameter(s) already exists!");
				return;
			}

			commands.Add(command);

			string ToString(List<string> list)
			{
				if (list.Count == 0)
				{
					return string.Empty;
				}
				
				StringBuilder stringBuilder = new StringBuilder(list[0]);

				for (int i = 1; i < list.Count; i++)
				{
					stringBuilder.Append($", {list[i]}");
				}

				return stringBuilder.ToString();
			}
		}

		public static void SetHelp(string helpCommand)
		{
			Command help = new Command(helpCommand, Help);
			help.SetHelpMessage(helpHelpMessage);

			Command<string> help1 = new Command<string>(helpCommand, Help);
			help1.SetHelpMessage(help1HelpMessage);

			AddCommand(help);
			AddCommand(help1);
		}

		private static void Help()
		{
			foreach (AbstractCommand command in commands)
			{
				ConsoleManager.Instance.Log(command.ToString());
			}
		}

		private static void Help(string commandName)
		{
			foreach (AbstractCommand command in GetCommands(commandName))
			{
				ConsoleManager.Instance.Log(command.ToString());
			}
		}
	}
}