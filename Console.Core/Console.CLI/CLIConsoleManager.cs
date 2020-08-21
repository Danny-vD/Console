using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;

namespace Console.CLI
{
    /// <summary>
    /// AConsoleManager implementation for CLI Programs.
    /// </summary>
    public class CLIConsoleManager : AConsoleManager
    {

        /// <summary>
        /// The Object Selector
        /// </summary>
        public override AObjectSelector ObjectSelector { get; }

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="extensions">Extensions to be Loaded</param>
        /// <param name="options">ConsoleInitOptions , Specifying the Default Commands available</param>
        public CLIConsoleManager(AExtensionInitializer[] extensions, ConsoleInitOptions options = ConsoleInitOptions.DefaultCommands) : this(options, false)
        {
            ExtensionLoader.ProcessLoadOrder(extensions);
            InvokeOnFinishInitialize();
        }

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="options">ConsoleInitOptions , Specifying the Default Commands available</param>
        /// <param name="runInit">Flag that specifies if the InvokeOnFinishInitialize() function will be invoked at the end of the constructor</param>
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
        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="extensionPath">Folder Path of the Extension Folder</param>
        /// <param name="options">ConsoleInitOptions , Specifying the Default Commands available</param>
        public CLIConsoleManager(string extensionPath, ConsoleInitOptions options = ConsoleInitOptions.DefaultCommands) : this(options, false)
        {
            ExtensionLoader.LoadFromFolder(extensionPath);
            InvokeOnFinishInitialize();
        }

        /// <summary>
        /// Clears the Console Window
        /// </summary>
        public override void Clear()
        {
            System.Console.Clear();
        }


        /// <summary>
        /// Writes a Log
        /// </summary>
        /// <param name="object">Object to Log</param>
        protected override void Log(object @object)
        {
            InvokeLogEvent(@object.ToString());
            System.Console.WriteLine(@object);
        }


        /// <summary>
        /// Writes a Log Error
        /// </summary>
        /// <param name="object">Error Object to Log</param>
        protected override void LogError(object @object)
        {
            Log(@object);
        }


        /// <summary>
        /// Writes an Unformatted Text
        /// </summary>
        /// <param name="text">Text to Log</param>
        public override void LogPlainText(string text)
        {
            Log(text);
        }

        /// <summary>
        /// Writes a Log Warning
        /// </summary>
        /// <param name="object">Warning Object to Log</param>
        protected override void LogWarning(object @object)
        {
            Log(@object);
        }
    }
}