using System;
using System.Collections.Generic;
using System.Text;
using Commands;

namespace Console
{
	public class CommandHandler
	{
		public void OnSubmitCommand(string command)
		{
			if (ConsoleManager.Instance.prefix != string.Empty && !command.StartsWith(ConsoleManager.Instance.prefix))
			{
				ConsoleManager.LogWarning(
					$"Invalid syntax! Type {ConsoleManager.Instance.prefix}{ConsoleManager.Instance.defaultCommands.helpCommand} to see a list of all commands!");
				return;
			}

			if (ConsoleManager.Instance.prefix != string.Empty)
			{
				command = command.Remove(0, ConsoleManager.Instance.prefix.Length);
			}

			ParseArguments(command);
		}

		private void ParseArguments(string arguments)
		{
			string[] commandArguments = Split(arguments, ' ');
			string commandName = commandArguments[0];

			if (commandArguments.Length == 1)
			{
				CommandManager.Invoke(commandName);
				return;
			}

			arguments = arguments.Remove(0, commandName.Length + 1); // +1 to also remove the whitespace

			if (arguments.Contains(ConsoleManager.Instance.stringChar.ToString()))
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
			// ReSharper disable once CoVariantArrayConversion
			// Reason: No danger as we're only reading from the array
			return Split(arguments, ' ');
		}

		private static string[] Split(string arguments, char split)
		{
			return arguments.Split(new[] {split}, StringSplitOptions.RemoveEmptyEntries);
		}

		private object[] GetCorrectParameters(string commandArguments)
		{
			commandArguments = commandArguments.Trim();
			string[] sections = commandArguments.Split(' ');

			// Split at whitespace, and append the items as long as a section does not contain an stringChar.
			string[] arguments = AppendStringArguments(sections);

			// ReSharper disable once CoVariantArrayConversion
			// Reason: No danger as we're only reading from the array
			return arguments;
		}

		private string[] AppendStringArguments(string[] parts)
		{
			bool append = false;
			StringBuilder stringBuilder = null;

			string stringSplit = ConsoleManager.Instance.stringChar.ToString();

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

			return arguments.ToArray();
		}
	}
}