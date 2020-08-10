using System;
using System.Reflection;
using Console.Core;
using Console.Core.PropertySystem;
using Console.Core.Utils;

namespace Console.ScriptSystem
{
    public class ScriptSystemInitializer : AExtensionInitializer
    {
        [Property("version.scriptsystem")]
        private static Version ScriptSystemVersion => Assembly.GetExecutingAssembly().GetName().Version;

        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<ScriptSystemInitializer>();
            CommandAttributeUtils.AddCommands<ScriptSystem>();
            PropertyAttributeUtils.AddProperties<ScriptSystem>();
        }
        
    }
}
