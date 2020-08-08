using Console.Core;
using Console.Core.Utils;

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
