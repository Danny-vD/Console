using System;
using System.Collections.Generic;
using Console.Core;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Attributes.CommandSystem.Helper;
using Console.Core.Attributes.PropertySystem;
using Console.Core.Attributes.PropertySystem.Helper;
using Console.Core.Commands.ConverterSystem;
using Console.Core.Console;
using Console.Core.ObjectSelection;
using Console.ObjectSelection;
using UnityEngine;
using UnityEngine.UI;
using VDUnityFramework.Standard.Singleton;

namespace Console.Console
{

    public class UnityConsoleManager : AConsoleManager
    {
        private ConsoleManager Manager;
        
        public UnityConsoleManager(ConsoleManager manager)
        {
            Manager = manager;
        }

        public void InvokeOnLog(string log) => InvokeLogEvent(log);
        public override void Log(object @object) => ConsoleManager.Log(@object);
        public override void LogWarning(object @object) => ConsoleManager.LogWarning(@object);
        public override void LogError(object @object) => ConsoleManager.LogError(@object);
        public override void Clear() => ConsoleManager.Clear();
        public override void LogCommand(string command) => ConsoleManager.LogCommand(command);
        public override void LogPlainText(string text) => ConsoleManager.LogPlainText(text);
        protected override void SubmitCommand(string command) => Manager.SubmitCommand(command);
        public override AObjectSelector ObjectSelector => Manager.ObjectSelector.Selector;
    }

    public class ConsoleManager : Singleton<ConsoleManager>
    {
        [ConsoleProperty("test.object")]
        public GameObject TestObject;
        [ConsoleProperty("test.list")]
        public List<GameObject> TestList;
        [ConsoleProperty("test.array")]
        public GameObject[] TestArray;

        private UnityConsoleManager Manager;

        public string ExtensionFolder = ".\\extensions\\";

        [Header("Components"), SerializeField] private InputField inputField = null;

        [SerializeField] private Text text = null;

        [SerializeField] private GameObject console = null;

        [SerializeField] private GameObject selectedObjectWindow = null;

        [Header("Command properties"), Space(20), Tooltip("Symbol(s) that must precede all commands")]
        public string prefix = "";

        [Tooltip("The character to tell the console that your argument is a string")]
        public char stringChar = '"';

        [Header("Console properties"), Tooltip("The combination of buttons to press to toggle the console")]
        public List<KeyCode> KeysToPress = new List<KeyCode>() { KeyCode.Home };

        [Space, SerializeField, Tooltip("The time (in seconds) before you can toggle the console again")]
        [ConsoleProperty("console.ui.cooldown")]
        private float toggleCooldown = 0.3f;
        
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

        [NonSerialized]
        public ObjectSelector ObjectSelector;

        private float time = 0;

        private ScrollRect scrollRect;

        private CommandHandler commandHandler;
        private DefaultLogReader logReader;

        private bool submittedCommand = false;
        private const ushort characterLimit = 10000;


        [Command("second", "")]
        private void TestCommandSecond(string test, [SelectionProperty] object value)
        {
            AConsoleManager.Instance.Log(value);
        }

        [Command("first", "")]
        private void TestCommandFirst([SelectionProperty] object[] value, string test)
        {
            for (int i = 0; i < value.Length; i++)
            {
                AConsoleManager.Instance.Log(value[i]);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            ConsoleCoreConfig.StringChar = stringChar;
            ConsoleCoreConfig.ConsolePrefix = prefix;

            Manager = new UnityConsoleManager(this);
            commandHandler = new CommandHandler();
            DefaultCommandAdder a = new DefaultCommandAdder();
            a.AddCommands();

            CommandAttributeUtils.AddCommands(this);

            ConsolePropertyAttributeUtils.InitializePropertySystem();
            ConsolePropertyAttributeUtils.AddProperties<ConsoleCoreConfig>();
            ConsolePropertyAttributeUtils.AddProperties(this);

            CustomConvertManager.AddConverter(new ArrayConverter());

            AExtensionInitializer.LoadExtensions(ExtensionFolder);



            DontDestroyOnLoad(true);

            ObjectSelector = GetComponent<ObjectSelector>();
            //commandHandler = new CommandHandler();
            logReader = new DefaultLogReader();
            
            scrollRect = console.GetComponentInChildren<ScrollRect>();
            
            
            inputField.onEndEdit.AddListener(Manager.EnterCommand);
            inputField.onEndEdit.AddListener(OnUIWriteCommand);

            ObjectSelector.enabled = console.activeSelf;
            selectedObjectWindow.SetActive(console.activeSelf);

            ConsolePropertyAttributeUtils.AddProperties(this);
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
                ObjectSelector.enabled = console.activeSelf;

                selectedObjectWindow.SetActive(console.activeSelf &&
                                               ObjectSelector.Selector.SelectedObjects.Count > 0);

                SetScrollbarToBottom();
            }

            Manager.InvokeOnTick();
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

            string txt = $"<color=#{Instance.normalColorHex}>{@object}\n" +
                         "---------------------------------------------------</color>\n";
            

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

            string txt = $"<color=#{Instance.warningColorHex}>{@object}</color>\n" +
                         $"<color=#{Instance.normalColorHex}>---------------------------------------------------</color>\n";
            
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

            string txt = $"<color=#{Instance.errorColorHex}>{@object}</color>\n" +
                         $"<color=#{Instance.normalColorHex}>---------------------------------------------------</color>\n";
            
            Instance.text.text += txt;
            Instance.Manager.InvokeOnLog(@object.ToString());

            MaintainCharacterLimit();
        }

        public static void LogCommand(string command)
        {
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