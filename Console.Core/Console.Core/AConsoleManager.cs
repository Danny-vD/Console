using System;
using Console.Core.CommandSystem;
using Console.Core.CommandSystem.Commands.BuiltIn;
using Console.Core.ExpanderSystem;
using Console.Core.PropertySystem;

/// <summary>
/// The Namespace Root of all Extensions and the Core Library.
/// </summary>
namespace Console
{

}
/// <summary>
/// The Root Namespace of the Console.Core Library.
/// </summary>
namespace Console.Core
{
    /// <summary>
    /// Abstract Console Manager Implementations.
    /// Has to be Implemented by the User.
    /// </summary>
    public abstract class AConsoleManager
    {
        [Flags]
        public enum ConsoleInitOptions
        {
            /// <summary>
            /// Add all Default Commands
            /// </summary>
            All = -1,
            /// <summary>
            /// Do not add any Default Commands
            /// </summary>
            None = 0,
            /// <summary>
            /// Clear / Help and Echo Command
            /// </summary>
            DefaultCommands = 1,
            /// <summary>
            /// Commands that allow interfacing with the Property System
            /// </summary>
            PropertyCommands = 2,
            /// <summary>
            /// Commands that allow Loading Extensions from the Commandline
            /// </summary>
            ExtensionCommands = 4,
            /// <summary>
            /// Commands that allow viewing and clearing the selected object list
            /// </summary>
            SelectionCommands = 8,
        }

        /// <summary>
        /// The Parser that handles the Command Input
        /// </summary>
        protected readonly CommandParser Parser;

        /// <summary>
        /// Has to be invoked for all Logs
        /// </summary>
        public static event Action<string> OnLog;

        /// <summary>
        /// Gets called when all Extensions were loaded.
        /// </summary>
        public static event Action OnInitializationFinished;

        /// <summary>
        /// "Hack" to provide a Console Tick Function that gets called periodically.
        /// Used in Networking Extension
        /// </summary>
        public static event Action OnConsoleTick;

        /// <summary>
        /// Expander System that allows to Expand parts of commands(Environment System)
        /// </summary>
        public static readonly ExpanderManager ExpanderManager = new ExpanderManager();

        /// <summary>
        /// Helper Function that has to get called every time a log gets sent(used by the networking system to transmit the logs)
        /// </summary>
        /// <param name="text">The Text to Log</param>
        protected void InvokeLogEvent(string text) => OnLog?.Invoke(text);

        /// <summary>
        /// Allows the Inheriting Class to invoke the OnInitializationFinished Event
        /// </summary>
        protected void InvokeOnFinishInitialize() => OnInitializationFinished?.Invoke();

        /// <summary>
        /// Singleton Instance of the Console
        /// </summary>
        public static AConsoleManager Instance { get; private set; }

        /// <summary>
        /// The Object Selector
        /// </summary>
        public abstract AObjectSelector ObjectSelector { get; }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="options">Console Initialization Flags.</param>
        protected AConsoleManager(ConsoleInitOptions options = ConsoleInitOptions.DefaultCommands)
        {
            Instance = this;
            Parser = new CommandParser();
            PropertyAttributeUtils.AddPropertiesByType(typeof(ConsoleCoreConfig));
            if ((options & ConsoleInitOptions.DefaultCommands) != 0)
                DefaultCommands.AddDefaultCommands();
            if ((options & ConsoleInitOptions.ExtensionCommands) != 0)
                ExtensionCommands.AddExtensionsCommands();
            if ((options & ConsoleInitOptions.PropertyCommands) != 0)
                PropertyCommands.AddPropertyCommands();
            if ((options & ConsoleInitOptions.SelectionCommands) != 0)
                ObjectSelectionCommands.AddSelectionCommands();
        }

        /// <summary>
        /// Clears the Console Logs
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Writes a Log
        /// </summary>
        /// <param name="object">Object to Log</param>
        public abstract void Log(object @object);
        /// <summary>
        /// Writes a Log Warning
        /// </summary>
        /// <param name="object">Object to Log</param>
        public abstract void LogWarning(object @object);
        /// <summary>
        /// Writes a Log Error
        /// </summary>
        /// <param name="object">Object to Log</param>
        public abstract void LogError(object @object);
        /// <summary>
        /// Writes the Entered Command into the Console Output if ConsoleCoreConfig.WriteCommand is set to true
        /// </summary>
        /// <param name="command">Command to Log</param>
        public virtual void LogCommand(string command)
        {
            if (ConsoleCoreConfig.WriteCommand) LogPlainText(command);
        }
        /// <summary>
        /// Writes Unaltered Plain text into the Console Window
        /// </summary>
        /// <param name="text">Text to Log</param>
        public abstract void LogPlainText(string text);

        /// <summary>
        /// Enters a Command into the Console.
        /// </summary>
        /// <param name="command">Command that is entered.</param>
        public void EnterCommand(string command)
        {
            string text = ExpanderManager.Expand(command);
            OnSubmitCommand(text);
            LogCommand(command);
            Parser.OnSubmitCommand(text);
        }

        /// <summary>
        /// Helper Function that allows Inheriting Classes to Invoke the OnTick event.
        /// </summary>
        public void InvokeOnTick() => OnConsoleTick?.Invoke();

        /// <summary>
        /// Function that can be used to react on any entered command
        /// </summary>
        /// <param name="command">Command that was entered</param>
        protected virtual void OnSubmitCommand(string command) { }
    }
}