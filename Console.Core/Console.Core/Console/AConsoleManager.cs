using System;
using Console.Core.ExpanderSystem;
using Console.Core.ObjectSelection;
using Console.Core.Utils;

namespace Console.Core.Console
{
    public abstract class AConsoleManager
    {
        protected readonly CommandHandler Handler;

        /// <summary>
        /// Has to be invoked for all Logs
        /// </summary>
        public static event Action<string> OnLog;
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
        /// <param name="text"></param>
        protected void InvokeLogEvent(string text) => OnLog?.Invoke(text);

        protected void InvokeOnFinishInitialize() => OnInitializationFinished?.Invoke();

        public static AConsoleManager Instance { get; private set; }

        public abstract AObjectSelector ObjectSelector { get; }

        protected AConsoleManager()
        {
            Instance = this;
            Handler = new CommandHandler();
            ConsoleCoreConfig.AddDefaultCommands();
            ConsolePropertyAttributeUtils.InitializePropertySystem();
        }

        public abstract void Clear();
        public abstract void Log(object @object);
        public abstract void LogWarning(object @object);
        public abstract void LogError(object @object);

        public virtual void LogCommand(string command)
        {
            if (ConsoleCoreConfig.WriteCommand) LogPlainText(command);
        }
        public abstract void LogPlainText(string text);

        public void EnterCommand(string command)
        {
            string text = ExpanderManager.Expand(command);
            SubmitCommand(text);
        }

        public void InvokeOnTick() => OnConsoleTick?.Invoke();

        protected abstract void SubmitCommand(string command);
    }
}