using System;
using System.Collections.Generic;
using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.ConverterSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;
using UnityEngine;
using UnityEngine.UI;
using PropertyAttribute = Console.Core.PropertySystem.PropertyAttribute;


/// <summary>
/// Namespace for all Unity Components for the Console System.
/// </summary>
namespace Console.Unity.Components
{

    /// <summary>
    /// Console Manager Component.
    /// Contains the AConsoleManager Implementation
    /// </summary>
    public class ConsoleManagerComponent : MonoBehaviour
    {
        public static ConsoleManagerComponent Instance { get; private set; }

        /// <summary>
        /// The AConsoleManager Implementation
        /// </summary>
        private UnityConsoleManager Manager;

        /// <summary>
        /// The Folder Containing the Extensions
        /// </summary>
        public string ExtensionFolder = ".\\extensions\\";
        /// <summary>
        /// The Log Seperator that is used after every log
        /// </summary>
        private static string Separator => Instance.useLogSeperator ? "\n---------------------------------------------------" : "";

        #region Components

        /// <summary>
        /// The Console Input Field
        /// </summary>
        [Header("Components"), SerializeField] private InputField inputField = null;

        /// <summary>
        /// The Console Output Text
        /// </summary>
        [SerializeField] private Text text = null;

        /// <summary>
        /// The Console Window
        /// </summary>
        [SerializeField] private GameObject console = null;

        /// <summary>
        /// The Selected Object Window
        /// </summary>
        [SerializeField] private GameObject selectedObjectWindow = null;

        #endregion

        #region Properties

        /// <summary>
        /// Init Options for the AConsoleManager Constructor
        /// </summary>
        public AConsoleManager.ConsoleInitOptions InitOptions = AConsoleManager.ConsoleInitOptions.All;

        /// <summary>
        /// The Prefix that should be used for every command
        /// </summary>
        [Header("Command properties"), Space(20), Tooltip("Symbol(s) that must precede all commands")]
        public string prefix = "";

        /// <summary>
        /// The String Char that defines where a string with spaces ends and starts
        /// </summary>
        [Tooltip("The character to tell the console that your argument is a string")]
        public char stringChar = '"';

        /// <summary>
        /// The Keys of which one needs to be pressed to toggle the console window
        /// </summary>
        [Header("Console properties"), Tooltip("The combination of buttons to press to toggle the console")]
        public List<KeyCode> KeysToPress = new List<KeyCode>() { KeyCode.Home };

        /// <summary>
        /// The Cooldown of the Console Toggle
        /// </summary>
        [Space, SerializeField, Tooltip("The time (in seconds) before you can toggle the console again")]
        [Property("unity.ui.cooldown")]
        private float toggleCooldown = 0.3f;

        
        /// <summary>
        /// The Color of Normal Logs
        /// </summary>
        [Space, SerializeField]
        [Property("unity.ui.normalcolor")]
        private string normalColorHex = "000000"; // Black

        /// <summary>
        /// The Color of Log Warnings
        /// </summary>
        [SerializeField]
        [Property("unity.ui.warningcolor")]
        private string warningColorHex = "D4D422"; // Yellow

        /// <summary>
        /// The Color of Log Errors
        /// </summary>
        [SerializeField]
        [Property("unity.ui.errorcolor")]
        private string errorColorHex = "882222"; // Red


        /// <summary>
        /// The Color of Written Commands
        /// </summary>
        [SerializeField]
        [Property("unity.ui.commandcolor")]
        private string commandColorHex = "FFFFFF"; // White

        /// <summary>
        /// If true it will append the LogSeperator string after every log
        /// </summary>
        [SerializeField]
        [Property("unity.ui.uselogseperator")]
        private bool useLogSeperator = true;
        #endregion

        #region Commands

        /// <summary>
        /// Exit Command
        /// </summary>
        [Command("exit", "Closes the application.", "Exit", "Quit", "quit")]
        private static void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }


        #endregion


        /// <summary>
        /// The Object Selector Component
        /// </summary>
        [NonSerialized]
        public ObjectSelectorComponent ObjectSelectorComponent;


        /// <summary>
        /// Cooldown Timer
        /// </summary>
        private float time = 0;
        /// <summary>
        /// Scroll Area of the Console Output
        /// </summary>
        private ScrollRect scrollRect;
        /// <summary>
        /// Log Reader Instance
        /// </summary>
        private DefaultLogReader logReader;

        /// <summary>
        /// Flag that determines if a Command is currently running in the Console System
        /// </summary>
        private bool submittedCommand = false;
        /// <summary>
        /// The Character limit of the console output
        /// </summary>
        private const ushort characterLimit = 10000;


        /// <summary>
        /// Initializes the Console System
        /// </summary>
        private void Awake()
        {
            Instance = this;

            ConsoleCoreConfig.StringChar = stringChar;
            ConsoleCoreConfig.ConsolePrefix = prefix;

            Manager = new UnityConsoleManager(InitOptions, this);

            CustomConvertManager.AddConverter(new GameObjectComponentConverter());

            //Console Setup
            CommandAttributeUtils.AddCommands(this);
            PropertyAttributeUtils.AddProperties(this);
            


            DontDestroyOnLoad(gameObject);

            ObjectSelectorComponent = GetComponent<ObjectSelectorComponent>();
            logReader = new DefaultLogReader();

            scrollRect = console.GetComponentInChildren<ScrollRect>();


            inputField.onEndEdit.AddListener(Manager.EnterCommand);
            inputField.onEndEdit.AddListener(OnUIWriteCommand);

            ObjectSelectorComponent.enabled = console.activeSelf;
            selectedObjectWindow.SetActive(console.activeSelf);

            ExtensionLoader.LoadFromFolder(ExtensionFolder);


            Manager.InvokeOnInitialize();
        }

        /// <summary>
        /// Updates the Console Window
        /// Console Toggle/...
        /// </summary>
        private void Update()
        {
            if (time > 0)
            {
                time -= Time.unscaledDeltaTime;
            }

            if (time <= 0 && KeysToPress.TrueForAll(Input.GetKey))
            {
                time = toggleCooldown;

                inputField?.Select();
                inputField?.ActivateInputField();

                console.SetActive(!console.activeSelf);
                ObjectSelectorComponent.enabled = console.activeSelf;

                selectedObjectWindow.SetActive(console.activeSelf &&
                                               ObjectSelectorComponent.Selector.SelectedObjects.Count > 0);

                SetScrollbarToBottom();
            }

            Manager.InvokeOnTick();
        }

        /// <summary>
        /// Object Selection Checks in LateUpdate
        /// </summary>
        private void LateUpdate()
        {
            if (submittedCommand)
            {
                ObjectSelectorComponent.CheckValid(); // In case the command modified the object
                submittedCommand = false;
            }
        }

        /// <summary>
        /// Releases the Resources of the Console System
        /// </summary>
        private void OnDestroy()
        {
            //commandHandler = null;

            logReader.Dispose();
            logReader = null;
            Instance = null;
        }

        /// <summary>
        /// Clears the Console Output
        /// </summary>
        public static void Clear()
        {
            Instance.text.text = string.Empty;
        }

        /// <summary>
        /// Writes a Log to the Console Output
        /// </summary>
        /// <param name="object">The Log to Write</param>
        /// <param name="logUnityConsole">If True will also log in the Unity Console</param>
        public static void Log(object @object, bool logUnityConsole = true)
        {
            if (logUnityConsole)
            {
                UnityEngine.Debug.Log(@object);
                return;
            }

            string txt = $"<color=#{Instance.normalColorHex}>{@object}" +
                         Separator + "</color>\n";


            Instance.text.text += txt;
            Instance.Manager.InvokeOnLog(@object.ToString());

            MaintainCharacterLimit();
        }

        /// <summary>
        /// Writes a Log Warning to the Console Output
        /// </summary>
        /// <param name="object">The Log Warning to Write</param>
        /// <param name="logUnityConsole">If True will also log in the Unity Console</param>
        public static void LogWarning(object @object, bool logUnityConsole = true)
        {

            if (logUnityConsole)
            {
                UnityEngine.Debug.LogWarning(@object);
                return;
            }

            string txt = $"<color=#{Instance.warningColorHex}>{@object}</color>" +
                         $"<color=#{Instance.normalColorHex}>" +
                         Separator + "</color>\n";

            Instance.text.text += txt;
            Instance.Manager.InvokeOnLog(@object.ToString());

            MaintainCharacterLimit();
        }



        /// <summary>
        /// Writes a Log Error to the Console Output
        /// </summary>
        /// <param name="object">The Log Error to Write</param>
        /// <param name="logUnityConsole">If True will also log in the Unity Console</param>
        public static void LogError(object @object, bool logUnityConsole = true)
        {
            if (logUnityConsole)
            {
                UnityEngine.Debug.LogError(@object);
                return;
            }

            string txt = $"<color=#{Instance.errorColorHex}>{@object}</color>" +
                         $"<color=#{Instance.normalColorHex}>" +
                         Separator + "</color>\n";

            Instance.text.text += txt;
            Instance.Manager.InvokeOnLog(@object.ToString());

            MaintainCharacterLimit();
        }

        /// <summary>
        /// Writes a Command into the Console Output
        /// </summary>
        /// <param name="command">Command to write</param>
        public static void LogCommand(string command)
        {
            if (!ConsoleCoreConfig.WriteCommand) return;
            string txt = $"<color=#{Instance.commandColorHex}>{command}</color>\n";

            Instance.text.text += txt;

            MaintainCharacterLimit();
        }

        /// <summary>
        /// Writes a Plain Text to the Console
        /// </summary>
        /// <param name="text">Text to Write</param>
        public static void LogPlainText(string text)
        {
            Instance.text.text += text;
            Instance.Manager.InvokeOnLog(text);
            MaintainCharacterLimit();
        }

        /// <summary>
        /// Gets Invoked when the InputField does lose focus.
        /// </summary>
        /// <param name="text"></param>
        private void OnUIWriteCommand(string text)
        {
            inputField?.Select();
            inputField?.ActivateInputField();
        }

        /// <summary>
        /// Handles the Console Window Update when a command got submitted.
        /// </summary>
        /// <param name="command"></param>
        public void SubmitCommand(string command)
        {
            if (command == string.Empty)
            {
                return;
            }

            LogCommand(command);
            inputField.text = string.Empty;
            //Manager.InvokeCommandHandler(command);

            submittedCommand = true;
        }

        /// <summary>
        /// Gets Invoked when the Console Output Got changed
        /// Moves the Cursor to the Last added String
        /// </summary>
        private void SetScrollbarToBottom()
        {
            if (scrollRect)
            {
                scrollRect.normalizedPosition = Vector2.zero;
            }
        }

        /// <summary>
        /// Enforces the Maximum Character Limit on the console output
        /// </summary>
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