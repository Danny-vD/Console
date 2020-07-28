using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VDFramework.Singleton;

namespace Console
{
	public class ConsoleManager : Singleton<ConsoleManager>
	{
		[Header("Components"), SerializeField]
		private InputField inputField = null;

		[SerializeField]
		private Text text = null;

		[SerializeField]
		private GameObject console = null;

		[Header("Command properties"), Space(20), Tooltip("Symbol(s) that must precede all commands")]
		public string prefix = "";

		[Tooltip("The character to tell the console that your argument is a string")]
		public char stringChar = '"';

		public DefaultCommandAdder defaultCommands;

		[Header("Console properties"), Tooltip("The combination of buttons to press to toggle the console")]
		public List<KeyCode> KeysToPress = new List<KeyCode>() {KeyCode.Home};

		[Space, SerializeField]
		private float toggleCooldown = 1;

		[Space, SerializeField]
		private string normalColorHex = "000000"; // Black

		[SerializeField]
		private string warningColorHex = "D4D422"; // Yellow

		[SerializeField]
		private string errorColorHex = "882222"; // Red

		[SerializeField]
		private string commandColorHex = "FFFFFF"; // White

		private float time = 0;

		private CommandHandler commandHandler;
		private DefaultLogReader logReader;

		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(true);

			commandHandler = new CommandHandler();
			defaultCommands = new DefaultCommandAdder();
			logReader = new DefaultLogReader();
			
			defaultCommands.AddCommands();

			inputField.onEndEdit.AddListener(OnSubmitCommand);
		}

		private void Update()
		{
			if (time > 0)
			{
				time -= Time.unscaledDeltaTime;
			}

			if (time <= 0 && KeysToPress.TrueForAll(Input.GetKey))
			{
				time = toggleCooldown;
				console.SetActive(!console.activeSelf);
			}
		}

		protected override void OnDestroy()
		{
			commandHandler = null;
			defaultCommands = null;
			
			logReader.OnDestroy();
			logReader = null;
			
			base.OnDestroy();
		}

		public static void Clear()
		{
			Instance.text.text = string.Empty;
		}

		public static void Log(object @object, bool logUnityConsole = true)
		{
			if (logUnityConsole)
			{
				UnityEngine.Debug.Log(@object);
				return;
			}
			
			Instance.text.text += $"<color=#{Instance.normalColorHex}>{@object}\n" +
								  "---------------------------------------------------</color>\n";
		}

		public static void LogWarning(object @object, bool logUnityConsole = true)
		{
			if (logUnityConsole)
			{
				UnityEngine.Debug.LogWarning(@object);
				return;
			}
			
			Instance.text.text += $"<color=#{Instance.warningColorHex}>{@object}</color>\n" +
								  $"<color=#{Instance.normalColorHex}>---------------------------------------------------</color>\n";
		}

		public static void LogError(object @object, bool logUnityConsole = true)
		{
			if (logUnityConsole)
			{
				UnityEngine.Debug.LogError(@object);
				return;
			}
			
			Instance.text.text += $"<color=#{Instance.errorColorHex}>{@object}</color>\n" +
								  $"<color=#{Instance.normalColorHex}>---------------------------------------------------</color>\n";
		}

		private static void LogCommand(string command)
		{
			Instance.text.text += $"<color=#{Instance.commandColorHex}>{command}</color>\n";
		}

		private void OnSubmitCommand(string command)
		{
			if (command == string.Empty)
			{
				return;
			}

			LogCommand(command);
			inputField.text = string.Empty;

			commandHandler.OnSubmitCommand(command);
		}
	}
}