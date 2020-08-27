using System.Collections.Generic;
using System.Linq;
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
        private Assembly ExtensionAssembly => GetType().Assembly;
        private static Dictionary<Assembly, ALogger> Loggers = new Dictionary<Assembly, ALogger>();

        public static ALogger GetLogger(Assembly asm) => Loggers.ContainsKey(asm) ? Loggers[asm] : null;

        public static void SetLogger(Assembly asm, ALogger logger)
        {
            Loggers[asm] = logger;
        }

        protected void SetLogger(ALogger logger) => SetLogger(ExtensionAssembly, logger);
        protected ALogger GetLogger() => GetLogger(ExtensionAssembly);

        public virtual LoadOrder Order => LoadOrder.Default;
        protected virtual string LoggerPrefix => ExtensionAssembly.GetName().Name;

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