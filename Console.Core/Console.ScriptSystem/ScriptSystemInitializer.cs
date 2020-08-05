using System;
using Console.Core;
using Console.Core.Attributes.CommandSystem.Helper;

namespace Console.ScriptSystem
{
    public class ScriptSystemInitializer : AExtensionInitializer
    {
        public override void Initialize()
        {
            CommandAttributeUtils.AddCommands<ScriptSystem>();
        }
    }
}
