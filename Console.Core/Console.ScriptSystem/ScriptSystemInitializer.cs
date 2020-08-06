using Console.Core;
using Console.Core.Attributes.CommandSystem.Helper;
using Console.Core.Attributes.PropertySystem.Helper;

namespace Console.ScriptSystem
{
    public class ScriptSystemInitializer : AExtensionInitializer
    {
        public override void Initialize()
        {
            CommandAttributeUtils.AddCommands<ScriptSystem>();
            ConsolePropertyAttributeUtils.AddProperties<ScriptSystem>();
        }
        
    }
}
