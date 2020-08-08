using Console.Core;
using Console.Core.Console;
using Console.Core.ExpanderSystem;
using Console.Core.Utils;

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