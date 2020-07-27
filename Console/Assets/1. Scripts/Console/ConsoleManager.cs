using System;
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


		[Space(20), SerializeField, Tooltip("Symbol(s) that must precede all commands")]
		private string prefix = "";

		[SerializeField, Tooltip("The command to display the help page")]
		private string helpCommand = "help";


		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(true);

			CommandManager.SetHelp(helpCommand);

			inputField.onEndEdit.AddListener(OnSubmitCommand);
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
			inputField.text = string.Empty;

			if (command == string.Empty || prefix != string.Empty && !command.StartsWith(prefix))
			{
				LogWarning($"Invalid syntax! Type {prefix}{helpCommand} to see a list of all commands!");
				return;
			}

			if (prefix != string.Empty)
			{
				command = command.Remove(0, prefix.Length);
			}

			string[] commandArguments = command.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

			ParseArguments(commandArguments);
		}

		private static void ParseArguments(string[] arguments)
		{
			if (arguments.Length == 1)
			{
				CommandManager.Invoke(arguments[0]);
				return;
			}

			object[] parameters = new object[arguments.Length - 1];

			for (int i = 0; i < parameters.Length; i++)
			{
				
			}
		}
	}
}