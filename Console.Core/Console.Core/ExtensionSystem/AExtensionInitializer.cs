using System.Collections.Generic;
using System.Reflection;

using Console.Core.ActivationSystem;
using Console.Core.LogSystem;

namespace Console.Core.ExtensionSystem
{
    /// <summary>
    /// Helper Class that has to be implemented with an empty public constructor to be detected and loaded by the console system
    /// </summary>
    [ActivateOn]
    public abstract class AExtensionInitializer
    {

        private static readonly Dictionary<Assembly, ALogger> Loggers = new Dictionary<Assembly, ALogger>();

        private Assembly ExtensionAssembly => GetType().Assembly;

        /// <summary>
        /// The Load Order of the Extension
        /// </summary>
        public virtual LoadOrder Order => LoadOrder.Default;

        /// <summary>
        /// The Logger Prefix for this Extension
        /// </summary>
        protected virtual string LoggerPrefix => ExtensionAssembly.GetName().Name;

        /// <summary>
        /// Returns the Logger with the Correct Prefix for the Assembly
        /// </summary>
        /// <param name="asm">The Assembly that the logger is created for.</param>
        /// <returns>Logger with Assembly Prefix</returns>
        public static ALogger GetLogger(Assembly asm)
        {
            return Loggers.ContainsKey(asm) ? Loggers[asm] : null;
        }

        /// <summary>
        /// Sets the Logger of a Specified Assembly
        /// </summary>
        /// <param name="asm">The Specified Assembly</param>
        /// <param name="logger">The Logger</param>
        public static void SetLogger(Assembly asm, ALogger logger)
        {
            Loggers[asm] = logger;
        }


        /// <summary>
        /// Sets the Logger of the Assembly
        /// </summary>
        /// <param name="logger">The Logger</param>
        protected void SetLogger(ALogger logger)
        {
            SetLogger(ExtensionAssembly, logger);
        }

        /// <summary>
        /// Returns the Logger of the ExtensionAssembly
        /// </summary>
        /// <returns>Logger for this Assembly</returns>
        protected ALogger GetLogger()
        {
            return GetLogger(ExtensionAssembly);
        }

        /// <summary>
        /// Initializes the Extensions in this Assembly.
        /// </summary>
        protected abstract void Initialize();

        internal void _InnerInitialize()
        {
            SetLogger(TypedLogger.CreateTypedWithPrefix(LoggerPrefix));
            Initialize();
        }

    }
}