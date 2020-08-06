using System;
using System.Collections.Generic;
using Console.Core;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Attributes.CommandSystem.Helper;
using Console.Core.Attributes.PropertySystem;
using Console.Core.Attributes.PropertySystem.Helper;
using Console.Core.Commands.ConverterSystem;
using Console.Core.Console;
using Console.ObjectSelection;
using UnityEngine;
using UnityEngine.UI;
using VDUnityFramework.Standard.Singleton;

namespace Console.Console
{
    public class ConsoleManagerComponent : Singleton<ConsoleManagerComponent>
    {

        private UnityConsoleManager Manager;

        public string ExtensionFolder = ".\\extensions\\";
        private static string Separator => Instance.useLogSeperator ? "\n---------------------------------------------------" : "";

        #region Components

        [Header("Components"), SerializeField] private InputField inputField = null;

        [SerializeField] private Text text = null;

        [SerializeField] private GameObject console = null;

        [SerializeField] private GameObject selectedObjectWindow = null;

        #endregion

        #region Properties

        [Header("Command properties"), Space(20), Tooltip("Symbol(s) that must precede all commands")]
        public string prefix = "";

        [Tooltip("The character to tell the console that your argument is a string")]
        public char stringChar = '"';

        [Header("Console properties"), Tooltip("The combination of buttons to press to toggle the console")]
        public List<KeyCode> KeysToPress = new List<KeyCode>() { KeyCode.Home };

        [Space, SerializeField, Tooltip("The time (in seconds) before you can toggle the console again")]
        [ConsoleProperty("unity.ui.cooldown")]
        private float toggleCooldown = 0.3f;

        [ConsoleProperty("test.object")]
        public GameObject testObject;
        [ConsoleProperty("test.list")]
        public List<GameObject> testList;
        [ConsoleProperty("test.array")]
        public GameObject[] testArray;

        [Space, SerializeField]
        [ConsoleProperty("unity.ui.normalcolor")]
        private string normalColorHex = "000000"; // Black

        [SerializeField]
        [ConsoleProperty("unity.ui.warningcolor")]
        private string warningColorHex = "D4D422"; // Yellow

        [SerializeField]
        [ConsoleProperty("unity.ui.errorcolor")]
        private string errorColorHex = "882222"; // Red

        [SerializeField]
        [ConsoleProperty("unity.ui.commandcolor")]
        private string commandColorHex = "FFFFFF"; // White

        [SerializeField]
        [ConsoleProperty("unity.ui.uselogseperator")]
        private bool useLogSeperator = true;
        #endregion

        #region Commands

        [Command("exit", "Closes the application.", "Exit", "Quit", "quit")]
        private void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }


        #endregion


        [NonSerialized]
        public ObjectSelectorComponent ObjectSelectorComponent;



        private float time = 0;
        private ScrollRect scrollRect;
        private DefaultLogReader logReader;
        private bool submittedCommand = false;
        private const ushort characterLimit = 10000;



        protected override void Awake()
        {
            base.Awake();

            ConsoleCoreConfig.StringChar = stringChar;
            ConsoleCoreConfig.ConsolePrefix = prefix;

            Manager = new UnityConsoleManager(this);

            CustomConvertManager.AddConverter(new GameObjectComponentConverter());

            //Console Setup
            CommandAttributeUtils.AddCommands(this);
            ConsolePropertyAttributeUtils.AddProperties(this);

            //Initializing Tests
            TestClass.InitializeTests();

            AExtensionInitializer.LoadExtensions(ExtensionFolder);



            DontDestroyOnLoad(true);

            ObjectSelectorComponent = GetComponent<ObjectSelectorComponent>();
            logReader = new DefaultLogReader();

            scrollRect = console.GetComponentInChildren<ScrollRect>();


            inputField.onEndEdit.AddListener(Manager.EnterCommand);
            inputField.onEndEdit.AddListener(OnUIWriteCommand);

            ObjectSelectorComponent.enabled = console.activeSelf;
            selectedObjectWindow.SetActive(console.activeSelf);



            Manager.InvokeOnInitialize();
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
        private void LateUpdate()
        {
            if (submittedCommand)
            {
                ObjectSelectorComponent.CheckValid(); // In case the command modified the object
                submittedCommand = false;
            }
        }

        protected override void OnDestroy()
        {
            //commandHandler = null;

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

            string txt = $"<color=#{Instance.normalColorHex}>{@object}" +
                         Separator + "</color>\n";


            Instance.text.text += txt;
            Instance.Manager.InvokeOnLog(@object.ToString());

            MaintainCharacterLimit();
        }

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

        public static void LogCommand(string command)
        {
            if (!ConsoleCoreConfig.WriteCommand) return;
            string txt = $"<color=#{Instance.commandColorHex}>{command}</color>\n";

            Instance.text.text += txt;

            MaintainCharacterLimit();
        }

        public static void LogPlainText(string text)
        {
            Instance.text.text += text;
            Instance.Manager.InvokeOnLog(text);
            MaintainCharacterLimit();
        }

        private void OnUIWriteCommand(string text)
        {
            inputField?.Select();
            inputField?.ActivateInputField();
        }


        public void SubmitCommand(string command)
        {
            if (command == string.Empty)
            {
                return;
            }

            LogCommand(command);
            inputField.text = string.Empty;
            Manager.InvokeCommandHandler(command);

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