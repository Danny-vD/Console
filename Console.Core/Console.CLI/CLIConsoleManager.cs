using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.CommandSystem.Commands.BuiltIn;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;

namespace Console.CLI
{
    public class CLIConsoleManager : AConsoleManager
    {
        public override AObjectSelector ObjectSelector { get; }

        public CLIConsoleManager(AExtensionInitializer[] extensions, ConsoleInitOptions options = ConsoleInitOptions.DefaultCommands) : this(options, false)
        {
            ExtensionLoader.ProcessLoadOrder(extensions);
            InvokeOnFinishInitialize();
        }

        public CLIConsoleManager(ConsoleInitOptions options, bool runInit = true) : base(options)
        {
            CLIObjSelector s = new CLIObjSelector();
            ObjectSelector = s;
            //CLI Specific Setup
            if ((options & ConsoleInitOptions.SelectionCommands) != 0)
                CLIObjectSelectionCommands.AddSelectionCommands();

            PropertyAttributeUtils.AddProperties<Program>();
            CommandAttributeUtils.AddCommands<Program>();

            if (runInit)
                InvokeOnFinishInitialize();

        }
        public CLIConsoleManager(string extensionPath, ConsoleInitOptions options = ConsoleInitOptions.DefaultCommands) : this(options, false)
        {
            ExtensionLoader.LoadFromFolder(extensionPath);
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
    }
}