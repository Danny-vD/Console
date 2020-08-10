using System;
using System.Reflection;
using Console.Core;
using Console.Core.Console;
using Console.Core.PropertySystem;
using Console.Core.Utils;

namespace Console.ScriptIOCompat
{
    public class ScriptIOCompatInitializer : AExtensionInitializer
    {
        [Property("version.scriptiocompat")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;

        [Property("scriptiocompat.scripts.autostart")]
        public static string AutoStartFile;
        public static void AutoStart()
        {

            AConsoleManager.Instance.Log("Running Auto Start Files: " + AutoStartFile);
            string[] files = AutoStartFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string file in files)
            {
                ScriptSystem.ScriptSystem.RunFile(file);
            }
        }


        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<ScriptIOCompatInitializer>();
            AConsoleManager.OnInitializationFinished += AutoStart;
        }
    }


}
