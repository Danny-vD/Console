using System;
using Console.Core.ObjectSelection;

namespace Console.Core.Console
{
    public abstract class AConsoleManager
    {
        /// <summary>
        /// Has to be invoked for all Logs
        /// </summary>
        public static event Action<string> OnLog;
        public static readonly ExpanderManager ExpanderManager = new ExpanderManager();
        public static event Action OnConsoleTick;

        protected void InvokeLogEvent(string text) => OnLog?.Invoke(text);
        public static AConsoleManager Instance { get; private set; }

        public abstract AObjectSelector ObjectSelector { get; set; }

        protected AConsoleManager()
        {
            Instance = this;
        }

        public abstract void Clear();
        public abstract void Log(object @object);
        public abstract void LogWarning(object @object);
        public abstract void LogError(object @object);
        public abstract void LogCommand(string command);
        public abstract void LogPlainText(string text);
        public void EnterCommand(string command)
        {
            string text = ExpanderManager.Expand(command);
            SubmitCommand(text);
        }

        public void InvokeOnTick() => OnConsoleTick?.Invoke();

        protected abstract void SubmitCommand(string command);
    }

    //public class ConsoleManager : AConsoleManager
    //{

    //    //[Header("Components"), SerializeField]
    //    //private InputField inputField = null;

    //    //[SerializeField]
    //    //private Text text = null;

    //    //[SerializeField]
    //    //private GameObject console = null;

    //    //[SerializeField]
    //    //private GameObject selectedObjectWindow = null;

    //    //[Header("Command properties"), Space(20), Tooltip("Symbol(s) that must precede all commands")]
    //    [ConsoleProperty("console.input.prefix")]
    //    //public string prefix = "";

    //    //[Tooltip("The character to tell the console that your argument is a string")]
    //    [ConsoleProperty("console.input.stringchar")]
    //    //public char stringChar = '"';

    //    public DefaultCommandAdder defaultCommands;

    //    ////[Header("Console properties"), Tooltip("The combination of buttons to press to toggle the console")]
    //    ////public List<KeyCode> KeysToPress = new List<KeyCode>() { KeyCode.Home };

    //    ////[Space, SerializeField, Tooltip("The time (in seconds) before you can toggle the console again")]
    //    [ConsoleProperty("console.ui.cooldown")]
    //    //private float toggleCooldown = 0.3f;

    //    ////      [Space, SerializeField]
    //    [ConsoleProperty("console.ui.normalcolor")]
    //    private string normalColorHex = "000000"; // Black

    //    ////[SerializeField]
    //    [ConsoleProperty("console.ui.warningcolor")]
    //    private string warningColorHex = "D4D422"; // Yellow

    //    ////[SerializeField]
    //    [ConsoleProperty("console.ui.errorcolor")]
    //    private string errorColorHex = "882222"; // Red

    //    ////[SerializeField]
    //    [ConsoleProperty("console.ui.commandcolor")]
    //    private string commandColorHex = "FFFFFF"; // White

    //    //[NonSerialized]
    //    public override AObjectSelector ObjectSelector { get; set; }

    //    private float time = 0;

    //    //private ScrollRect scrollRect;

    //    private CommandHandler commandHandler;
    //    //private DefaultLogReader logReader;

    //    private bool submittedCommand = false;
    //    private const ushort characterLimit = 10000;

    //    public ConsoleManager()
    //    {
    //        commandHandler = new CommandHandler();
    //        defaultCommands = new DefaultCommandAdder();
    //        //logReader = new DefaultLogReader();
    //        defaultCommands.AddCommands();
    //    }

    //    //protected override void Awake()
    //    //{
    //    //    base.Awake();
    //    //    DontDestroyOnLoad(true);

    //    //    ObjectSelector = GetComponent<ObjectSelector>();
    //    //    commandHandler = new CommandHandler();
    //    //    defaultCommands = new DefaultCommandAdder();
    //    //    logReader = new DefaultLogReader();

    //    //    scrollRect = console.GetComponentInChildren<ScrollRect>();

    //    //    defaultCommands.AddCommands();

    //    //    inputField.onEndEdit.AddListener(OnSubmitCommand);
    //    //    inputField.onEndEdit.AddListener(OnUIWriteCommand);

    //    //    ObjectSelector.enabled = console.activeSelf;
    //    //    selectedObjectWindow.SetActive(console.activeSelf);

    //    //    ConsolePropertyAttributeUtils.AddProperties(this);
    //    //}

    //    //private void Update()
    //    //{
    //    //    if (time > 0)
    //    //    {
    //    //        time -= Time.unscaledDeltaTime;
    //    //    }

    //    //    if (time <= 0 && KeysToPress.TrueForAll(Input.GetKey))
    //    //    {
    //    //        time = toggleCooldown;

    //    //        inputField?.Select();
    //    //        inputField?.ActivateInputField();

    //    //        console.SetActive(!console.activeSelf);
    //    //        ObjectSelector.enabled = console.activeSelf;

    //    //        selectedObjectWindow.SetActive(console.activeSelf &&
    //    //                                       ObjectSelector.SelectedObjects.Count > 0);

    //    //        SetScrollbarToBottom();
    //    //    }
    //    //}

    //    //private void LateUpdate()
    //    //{
    //    //    if (submittedCommand)
    //    //    {
    //    //        ObjectSelector.CheckValid(); // In case the command modified the object
    //    //        submittedCommand = false;
    //    //    }
    //    //}

    //    public void Dispose()
    //    {
    //        commandHandler = null;
    //        defaultCommands = null;

    //        //logReader.OnDestroy();
    //        //logReader = null;
    //    }

    //    //protected override void OnDestroy()
    //    //{
    //    //    commandHandler = null;
    //    //    defaultCommands = null;

    //    //    logReader.OnDestroy();
    //    //    logReader = null;

    //    //    base.OnDestroy();
    //    //}

    //    public override void Clear()
    //    {
    //        System.Console.Clear();
    //        //Instance.text.text = string.Empty;
    //    }

    //    public override void EnterCommand(string command)
    //    {
    //        OnSubmitCommand(command);
    //    }

    //    public override void Log(object @object)
    //    {
    //        string txt = $"<color=#{normalColorHex}>{@object}\n" +
    //                     "---------------------------------------------------</color>\n";
    //        OnLog?.Invoke(txt);

    //        System.Console.WriteLine(txt);
    //        //Instance.text.text += txt;

    //        //MaintainCharacterLimit();
    //    }

    //    public override void LogWarning(object @object)
    //    {
    //        string txt = $"<color=#{warningColorHex}>{@object}</color>\n" +
    //                     $"<color=#{normalColorHex}>---------------------------------------------------</color>\n";
    //        OnLog?.Invoke(txt);
    //        System.Console.WriteLine(txt);
    //        //text.text += txt;

    //        //MaintainCharacterLimit();
    //    }

    //    public override void LogError(object @object)
    //    {
    //       string txt = $"<color=#{errorColorHex}>{@object}</color>\n" +
    //                     $"<color=#{normalColorHex}>---------------------------------------------------</color>\n";
    //        OnLog?.Invoke(txt);
    //        System.Console.WriteLine(txt);
    //        //text.text += txt;

    //        //MaintainCharacterLimit();
    //    }

    //    public override void LogCommand(string command)
    //    {
    //        string txt = $"<color=#{commandColorHex}>{command}</color>\n";
    //        OnLog?.Invoke(txt);

    //        System.Console.WriteLine(txt);
    //        //text.text += txt;

    //    }

    //    public override void LogPlainText(string text)
    //    {

    //        System.Console.WriteLine(text);
    //        //text.text += text;
    //    }

    //    //private void OnUIWriteCommand(string text)
    //    //{
    //    //    inputField?.Select();
    //    //    inputField?.ActivateInputField();
    //    //}

    //    private void OnSubmitCommand(string command)
    //    {
    //        if (command == string.Empty)
    //        {
    //            return;
    //        }

    //        string cmd = OnCommandSubmit?.Invoke(command);
    //        //string cmd = EnvironmentVariableManager.Expand(command);

    //        LogCommand(command);
    //        //inputField.text = string.Empty;

    //        commandHandler.OnSubmitCommand(cmd);

    //        submittedCommand = true;
    //    }

    //    //private void SetScrollbarToBottom()
    //    //{
    //    //    if (scrollRect)
    //    //    {
    //    //        scrollRect.normalizedPosition = Vector2.zero;
    //    //    }
    //    //}

    //    //private static void MaintainCharacterLimit()
    //    //{
    //    //    string textString = text.text;

    //    //    ushort length = (ushort)textString.Length;

    //    //    if (length <= characterLimit)
    //    //    {
    //    //        return;
    //    //    }

    //    //    int startIndex = length - characterLimit;
    //    //    startIndex = textString.IndexOf("</color>", startIndex, StringComparison.Ordinal) + "</color>\n".Length;

    //    //    text.text = textString.Substring(startIndex, length - startIndex);
    //    //}
    //}
}