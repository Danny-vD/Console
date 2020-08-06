using System;
using Console.Core;
using Console.Core.Attributes.PropertySystem;
using Console.Core.Attributes.PropertySystem.Helper;
using Console.Core.Console;

namespace Console.ScriptIOCompat
{
    public class ScriptIOCompatInitializer : AExtensionInitializer
    {

        [ConsoleProperty("console.scripts.autostart")]
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
            ConsolePropertyAttributeUtils.AddProperties<ScriptIOCompatInitializer>();
            AConsoleManager.OnInitializationFinished += AutoStart;
        }
    }


}
