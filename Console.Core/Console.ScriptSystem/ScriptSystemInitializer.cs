using System;
using System.Reflection;
using Console.Core.CommandSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;

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
