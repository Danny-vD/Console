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
            ScriptSystem.ScriptSystem.RunFile(AutoStartFile);
        }


        public override void Initialize()
        {
            ConsolePropertyAttributeUtils.AddProperties<ScriptIOCompatInitializer>();
            AConsoleManager.OnInitializationFinished += AutoStart;
        }
    }


}
