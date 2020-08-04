using System;
using System.Collections.Generic;
using Console.ObjectSelection;
using UnityEngine;
using UnityEngine.UI;
using VDUnityFramework.Standard.Singleton;

namespace Console.Console
{
    public class ConsoleManager : Singleton<ConsoleManager>
    {
        public static event Action<string> OnLog;

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
        public List<KeyCode> KeysToPress = new List<KeyCode>() { KeyCode.Home };

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
        private const ushort characterLimit = 10000;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(true);

            ObjectSelector = GetComponent<ObjectSelector>();
            commandHandler = new CommandHandler();
            defaultCommands = new DefaultCommandAdder();
            logReader = new DefaultLogReader();

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

                selectedObjectWindow.SetActive(console.activeSelf &&
                                               ObjectSelector.SelectedObjects.Count > 0);

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

            string txt = $"<color=#{Instance.normalColorHex}>{@object}\n" +
                         "---------------------------------------------------</color>\n";
            OnLog?.Invoke(txt);

            Instance.text.text += txt;

            MaintainCharacterLimit();
        }

        public static void LogWarning(object @object, bool logUnityConsole = true)
        {
            if (logUnityConsole)
            {
                UnityEngine.Debug.LogWarning(@object);
                return;
            }

            string txt = $"<color=#{Instance.warningColorHex}>{@object}</color>\n" +
                         $"<color=#{Instance.normalColorHex}>---------------------------------------------------</color>\n";
            OnLog?.Invoke(txt);
            Instance.text.text += txt;

            MaintainCharacterLimit();
        }

        public static void LogError(object @object, bool logUnityConsole = true)
        {
            if (logUnityConsole)
            {
                UnityEngine.Debug.LogError(@object);
                return;
            }

            string txt = $"<color=#{Instance.errorColorHex}>{@object}</color>\n" +
                         $"<color=#{Instance.normalColorHex}>---------------------------------------------------</color>\n";
            OnLog?.Invoke(txt);
            Instance.text.text += txt;

            MaintainCharacterLimit();
        }

        private static void LogCommand(string command)
        {
            string txt = $"<color=#{Instance.commandColorHex}>{command}</color>\n";
            OnLog?.Invoke(txt);
            Instance.text.text += txt;

        }

        public static void LogPlainText(string text)
        {
            Instance.text.text += text;
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

        private static void MaintainCharacterLimit()
        {
            string textString = Instance.text.text;

            ushort length = (ushort)textString.Length;

            if (length <= characterLimit)
            {
                return;
            }

            int startIndex = length - characterLimit;
            startIndex = textString.IndexOf("</color>", startIndex, StringComparison.Ordinal) + "</color>\n".Length;

            Instance.text.text = textString.Substring(startIndex, length - startIndex);
        }
    }
}