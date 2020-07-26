using System;
using Commands;
using UnityEngine;
using VDFramework.Extensions;
using VDFramework.Singleton;

namespace Console
{
	public class ConsoleManager : Singleton<ConsoleManager>
	{
		[SerializeField, Tooltip("Symbol(s) that must precede all commands")]
		private string prefix = "";

		[SerializeField, Tooltip("The command to display the help page")]
		private string helpCommand = "help";

		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(true);

			CommandManager.SetHelp(helpCommand);

			Temporary();
		}

		private void Temporary()
		{
			CommandManager.AddCommand(new Command<int>("Log", Test));
			CommandManager.Invoke("Log", true);
			
			void Test(int f)
			{
				Log("Performed work");
			}
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
	}
}