using System;
using System.Reflection;

using Console.Core;
using Console.Core.ExtensionSystem;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;
using Console.Script;


/// <summary>
/// Compatibility Layer for the Script System Extension and the IO Extension
/// Allows execution of a Startup Script
/// </summary>
namespace Compat.Script.IO
{
    /// <summary>
    /// Initializer of the ScriptIOCompat Extension
    /// </summary>
    public class ScriptIOCompatInitializer : AExtensionInitializer
    {

        /// <summary>
        /// The ; Seperated List of scripts to run after the console initialized.
        /// </summary>
        [Property("compat.script.io.autostart")]
        public static string AutoStartFile;

        [Property("logs.compat.script.io.mute")]
        private static bool MuteLogs
        {
            get => Logger.Mute;
            set => Logger.Mute = value;
        }

        /// <summary>
        /// The Logger for this Extension
        /// </summary>
        public static ALogger Logger => GetLogger(Assembly.GetExecutingAssembly());

        /// <summary>
        /// Version of the ScriptIOCompat Extension
        /// </summary>
        [Property("version.compat.script.io")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Autostart Routine.
        /// Does Load the ; separated property scriptiocompat.scripts.autostart and executes all scripts in this list in sequence
        /// </summary>
        public static void AutoStart()
        {
            if (AutoStartFile == null)
            {
                return;
            }

            Logger.Log("Running Auto Start Files: " + AutoStartFile);
            string[] files = AutoStartFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string file in files)
            {
                ScriptSystem.RunFile(file);
            }
        }


        /// <summary>
        /// Initialization Function
        /// </summary>
        protected override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<ScriptIOCompatInitializer>();
            AConsoleManager.OnInitializationFinished += AutoStart;
        }

    }
}