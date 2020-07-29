using System;
using System.Collections.Generic;
using Console.ObjectSelection;
using UnityEngine;
using UnityEngine.UI;
using VDFramework.Singleton;

namespace Console.Console
{
	public class ConsoleManager : Singleton<ConsoleManager>
	{
		[Header("Components"), SerializeField]
		private InputField inputField = null;

		[SerializeField]
		private Text text = null;

		[SerializeField]
		private GameObject console = null;

		[SerializeField]
		private GameObject selectedObjectWindow = null;

		[Header("Command properties"), Space(20), Tooltip("Symbol(s) that must precede all commands")]
		public string prefix = "";

		[Tooltip("The character to tell the console that your argument is a string")]
		public char stringChar = '"';

		public DefaultCommandAdder defaultCommands;

		[Header("Console properties"), Tooltip("The combination of buttons to press to toggle the console")]
		public List<KeyCode> KeysToPress = new List<KeyCode>() {KeyCode.Home};

		[Space, SerializeField, Tooltip("The time (in seconds) before you can toggle the console again")]
		private float toggleCooldown = 0.3f;

		[Space, SerializeField]
		private string normalColorHex = "000000"; // Black

		[SerializeField]
		private string warningColorHex = "D4D422"; // Yellow

		[SerializeField]
		private string errorColorHex = "882222"; // Red

		[SerializeField]
		private string commandColorHex = "FFFFFF"; // White

		[NonSerialized]
		public ObjectSelector ObjectSelector;

		private float time = 0;

		private ScrollRect scrollRect;

		private CommandHandler commandHandler;
		private DefaultLogReader logReader;

		private bool submittedCommand = false;

		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(true);

			ObjectSelector  = GetComponent<ObjectSelector>();
			commandHandler  = new CommandHandler();
			defaultCommands = new DefaultCommandAdder();
			logReader       = new DefaultLogReader();

			scrollRect = console.GetComponentInChildren<ScrollRect>();

			defaultCommands.AddCommands();

			inputField.onEndEdit.AddListener(OnSubmitCommand);

			ObjectSelector.enabled = console.activeSelf;
			selectedObjectWindow.SetActive(console.activeSelf);
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
				ObjectSelector.enabled = console.activeSelf;
				selectedObjectWindow.SetActive(console.activeSelf);

				SetScrollbarToBottom();
			}
		}

		private void LateUpdate()
		{
			if (submittedCommand)
			{
				ObjectSelector.CheckValid(); // In case the command modified the object
				submittedCommand = false;
			}
		}

		protected override void OnDestroy()
		{
			commandHandler  = null;
			defaultCommands = null;

			logReader.OnDestroy();
			logReader = null;

			base.OnDestroy();
		}

		public static void Clear()
		{
			Instance.text.text = string.Empty;
		}

		public static void EnterCommand(string command)
		{
			Instance.OnSubmitCommand(command);
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

			submittedCommand = true;
		}

		private void SetScrollbarToBottom()
		{
			if (scrollRect)
			{
				scrollRect.normalizedPosition = Vector2.zero;
			}
		}
	}
}