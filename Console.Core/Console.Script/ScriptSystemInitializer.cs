using System;
using System.Reflection;

using Console.Core.ActivationSystem;
using Console.Core.CommandSystem.Attributes;
using Console.Core.ExtensionSystem;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;
using Console.Script.Deblocker;
using Console.Script.Deblocker.Implementations;
using Console.Script.Deblocker.Parameters;
using Console.Script.Extensions;
using Console.Vars;

/// <summary>
/// The Script System Extension implements functionality that allows running of scripts as if they were typed in the console input line by line.
/// </summary>
namespace Console.Script
{
    /// <summary>
    /// Initializer of the ScriptSystem Extension
    /// </summary>
    public class ScriptSystemInitializer : AExtensionInitializer
    {

        /// <summary>
        /// 
        /// </summary>
        public const string SCRIPT_SYSTEM_NAMESPACE = "script";

        /// <summary>
        /// Mutes all Script System Logs
        /// </summary>
        [Property("logs."+ SCRIPT_SYSTEM_NAMESPACE + ".mute")]
        private static bool MuteLogs
        {
            get => Logger.Mute;
            set => Logger.Mute = value;
        }

        /// <summary>
        /// Logger used by the Script System
        /// </summary>
        public static ALogger Logger => TypedLogger.CreateTypedWithPrefix(SCRIPT_SYSTEM_NAMESPACE);

        /// <summary>
        /// Version of the ScriptSystem Extension
        /// </summary>
        [Property("version."+ SCRIPT_SYSTEM_NAMESPACE)]
        private static Version ScriptSystemVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Initialization Function
        /// </summary>
        protected override void Initialize()
        {
            AExtensionFilter.AddFilter(new BlackListFilter());
            AExtensionFilter.AddFilter(new WhiteListFilter(new[] { "txt", "csf" }));

            EnvironmentVariableManager.AddProvider(new ParameterVariableContainer());
            PropertyAttributeUtils.AddProperties<ScriptSystemInitializer>();
            CommandAttributeUtils.AddCommands(typeof(ScriptSystem));
            CommandAttributeUtils.AddCommands(typeof(SequenceSystem));
            PropertyAttributeUtils.AddProperties(typeof(ScriptSystem));
            PropertyAttributeUtils.AddProperties(typeof(DeblockerSettings));

            ADeblocker[] db = ActivateOnAttributeUtils.ActivateObjects<ADeblocker>(Assembly.GetExecutingAssembly());
            foreach (ADeblocker aDeblocker in db)
            {
                if (aDeblocker.GetType() != typeof(DefaultDeblocker))
                {
                    DeblockerCollection.AddDeblocker(aDeblocker);
                }
            }
        }

    }
}