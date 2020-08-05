using System;
using Console.Core.Commands.ExpanderSystem;
using Console.Core.ObjectSelection;

namespace Console.Core.Console
{
    public abstract class AConsoleManager
    {
        /// <summary>
        /// Has to be invoked for all Logs
        /// </summary>
        public static event Action<string> OnLog;

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


        public static AConsoleManager Instance { get; private set; }

        public abstract AObjectSelector ObjectSelector { get; }

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
}