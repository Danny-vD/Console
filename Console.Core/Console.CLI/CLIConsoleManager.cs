using Console.Core;
using Console.Core.Console;
using Console.Core.ObjectSelection;
using Console.Core.Utils;

namespace Console.CLI
{
    public class CLIConsoleManager : AConsoleManager
    {
        public override AObjectSelector ObjectSelector { get; }

        public CLIConsoleManager(AExtensionInitializer[] extensions) : this(false)
        {
            ConsoleCoreConfig.LoadExtensions(extensions);
            InvokeOnFinishInitialize();
        }

        public CLIConsoleManager(bool runInit = true)
        {

            ObjectSelector = new CLIObjSelector();
            //CLI Specific Setup
            ConsolePropertyAttributeUtils.AddProperties<Program>();
            TestClass.InitializeTests();
            CommandAttributeUtils.AddCommands<Program>();

            if (runInit)
                InvokeOnFinishInitialize();

        }
        public CLIConsoleManager(string extensionPath) : this(false)
        {
            AExtensionInitializer.LoadExtensions(extensionPath);
            InvokeOnFinishInitialize();
        }

        public override void Clear()
        {
            System.Console.Clear();
        }

        public override void Log(object @object)
        {
            InvokeLogEvent(@object.ToString());
            System.Console.WriteLine(@object);
        }

        public override void LogError(object @object)
        {
            Log("Error: " + @object);
        }

        public override void LogPlainText(string text)
        {
            Log(text);
        }

        public override void LogWarning(object @object)
        {
            Log("Warning: " + @object);
        }

        protected override void SubmitCommand(string command)
        {
            string cmd = ExpanderManager.Expand(command);
            LogCommand(command);
            Handler.OnSubmitCommand(cmd);
        }
    }
}