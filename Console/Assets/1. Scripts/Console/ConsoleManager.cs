using System;
using System.Collections.Generic;
using System.Text;
using Commands;
using UnityEngine;
using UnityEngine.UI;
using VDFramework.Singleton;

namespace Console
{
	public class ConsoleManager : Singleton<ConsoleManager>
	{
		[SerializeField]
		private InputField inputField = null;

		[SerializeField]
		private Text text = null;

		[Space(20), Tooltip("Symbol(s) that must precede all commands")]
		public string prefix = "";

		[Tooltip("The character to tell the console that your argument is a string")]
		public char stringChar = '"';

		[Tooltip("The command to display the help page")]
		public string helpCommand = "help";

		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(true);

			CommandManager.SetHelp(helpCommand);

			inputField.onEndEdit.AddListener(OnSubmitCommand);

			CommandManager.AddCommand(new Command<string, string>("Write", Write));
			CommandManager.AddCommand(new Command<byte, byte>("Add", Add));
		}

		private void Write(string newText, string test)
		{
			string toWrite = string.Empty;

			toWrite += newText + "\n" + test + "\n";

			text.text = toWrite;
		}

		private void Add(byte a, byte b)
		{
			text.text = (a + b).ToString();
		}

		public void Log(object @object)
		{
			//Write it to the text

			UnityEngine.Debug.Log(@object);
		}

		public void LogWarning(object @object)
		{
			UnityEngine.Debug.LogWarning(@object);
		}

		public void LogError(object @object)
		{
			UnityEngine.Debug.LogError(@object);
		}

		private void OnSubmitCommand(string command)
		{
			if (command == string.Empty)
			{
				return;
			}

			inputField.text = string.Empty;

			if (prefix != string.Empty && !command.StartsWith(prefix))
			{
				LogWarning($"Invalid syntax! Type {prefix}{helpCommand} to see a list of all commands!");
				return;
			}

			if (prefix != string.Empty)
			{
				command = command.Remove(0, prefix.Length);
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

			if (arguments.Contains(stringChar.ToString()))
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
			// ReSharper disable once CoVariantArrayConversion || Reason: No danger as we're only reading from the array
			return Split(arguments, ' ');
		}

		private static string[] Split(string arguments, char split)
		{
			return arguments.Split(new[] {split}, StringSplitOptions.RemoveEmptyEntries);
		}

		private object[] GetCorrectParameters(string commandArguments)
		{
			// Just split at the space, and go in some kind of 'append' mode as long as a section does not contain an stringChar.
			commandArguments = commandArguments.Trim();
			string[] sections = commandArguments.Split(' ');

			string[] arguments = AppendStringArguments(sections); 
			
			// ReSharper disable once CoVariantArrayConversion || Reason: No danger as we're only reading from the array
			return arguments;
		}

		private string[] AppendStringArguments(string[] parts)
		{
			bool append = false;
			StringBuilder stringBuilder = null;

			string stringSplit = stringChar.ToString();
			
			List<string> arguments = new List<string>();
			
			for (int i = 0; i < parts.Length; i++)
			{
				bool containsStringChar = parts[i].Contains(stringSplit);

				if (!append && containsStringChar)
				{
					append = true;
					stringBuilder = new StringBuilder(parts[i]);

					if (parts[i].EndsWith(stringSplit))
					{
						string argumentString = stringBuilder.ToString();
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
						arguments.Add(argumentString.Replace(stringSplit, ""));
						append = false;
					}
					
					continue;
				}
				
				arguments.Add(parts[i]);
			}

			return arguments.ToArray();
		}
	}
}