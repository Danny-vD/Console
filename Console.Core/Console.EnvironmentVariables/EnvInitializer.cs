using Console.Core;
using Console.Core.Attributes.CommandSystem.Helper;
using Console.Core.Commands.ExpanderSystem;
using Console.Core.Console;

namespace Console.EnvironmentVariables
{
    public class EnvInitializer : AExtensionInitializer
    {
        private class EnvExpander : AExpander
        {
            public override string Expand(string input)
            {
                return EnvironmentVariableManager.Expand(input);
            }
        }

        public override void Initialize()
        {
            EnvironmentVariableManager.AddProvider(DefaultVariables.Instance);
            CommandAttributeUtils.AddCommands(typeof(EnvironmentVariableManager));
            AConsoleManager.ExpanderManager.AddExpander(new EnvExpander());
        }
    }
}