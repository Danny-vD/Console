using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Core.Commands;

namespace Console.Core.Console
{
	public class CommandHandler
	{
		public void OnSubmitCommand(string command)
		{
			if (ConsoleCoreConfig.ConsolePrefix != string.Empty && !command.StartsWith(ConsoleCoreConfig.ConsolePrefix))
			{
				AConsoleManager.Instance.LogWarning(
					$"Command does not start with prefix: "+ ConsoleCoreConfig.ConsolePrefix);
				return;
			}

			if (ConsoleCoreConfig.ConsolePrefix != string.Empty)
			{
				command = command.Remove(0, ConsoleCoreConfig.ConsolePrefix.Length);
			}

			ParseArguments(command);
		}

		private static void ParseArguments(string arguments)
		{
			string[] commandArguments = Split(arguments, ' ');
			string commandName = commandArguments[0];

			if (commandArguments.Length == 1 &&
				AConsoleManager.Instance.ObjectSelector.SelectedObjects.Count == 0)
			{
				CommandManager.Invoke(commandName);
				return;
			}

			arguments = arguments.Remove(0, commandName.Length);
			arguments = arguments.Trim();

			if (arguments.Contains(ConsoleCoreConfig.StringChar.ToString()))
			{
				CommandManager.Invoke(commandName, GetCorrectParameters(arguments));
			}
			else
			{
				CommandManager.Invoke(commandName, ProcessArguments(arguments));
			}
		}

		private static object[] ProcessArguments(string arguments)
		{
			//List<object> parameters = AConsoleManager.Instance.ObjectSelector.SelectedObjects;
			
			List<string> commandArguments = Split(arguments, ' ').ToList();
			
			//commandArguments.ForEach(parameters.Add);

			return commandArguments.ToArray();
		}

		private static string[] Split(string arguments, char split)
		{
			return arguments.Split(new[] {split}, StringSplitOptions.RemoveEmptyEntries);
		}

		private static object[] GetCorrectParameters(string commandArguments)
		{
			string[] sections = commandArguments.Split(' ');

			// Split at whitespace, and append the items as long as a section does not contain an stringChar.
			List<string> arguments = AppendStringArguments(sections);

			List<object> parameters = AConsoleManager.Instance.ObjectSelector.SelectedObjects.ToList();
			
			arguments.ForEach(parameters.Add);
			
			return parameters.ToArray();
		}

		private static List<string> AppendStringArguments(string[] parts)
		{
			bool append = false;
			StringBuilder stringBuilder = null;

			string stringSplit = ConsoleCoreConfig.StringChar.ToString();

			List<string> arguments = new List<string>();

			for (int i = 0; i < parts.Length; i++)
			{
				bool containsStringChar = parts[i].Contains(stringSplit);

				if (!append && containsStringChar)
				{
					append        = true;
					stringBuilder = new StringBuilder(parts[i]);

					if (parts[i].EndsWith(stringSplit))
					{
						string argumentString = stringBuilder.ToString();
						stringBuilder.Clear();
						arguments.Add(argumentString.Replace(stringSplit, ""));
						append = false;
					}

					continue;
				}

				if (append)
				{
					stringBuilder.Append($" {parts[i]}");

					if (containsStringChar)
					{
						string argumentString = stringBuilder.ToString();
						stringBuilder.Clear();
						arguments.Add(argumentString.Replace(stringSplit, ""));
						append = false;
					}

					continue;
				}

				arguments.Add(parts[i]);
			}

			// ReSharper disable once PossibleNullReferenceException
			// Reason: false positive (it's guaranteed to be initialised)
			if (stringBuilder.Length != 0)
			{
				string argumentString = stringBuilder.ToString();
				arguments.Add(argumentString.Replace(stringSplit, ""));
			}

			return arguments;
		}
	}
}