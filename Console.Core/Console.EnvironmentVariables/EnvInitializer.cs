using System;
using System.Reflection;
using Console.Core;
using Console.Core.Console;
using Console.Core.ExpanderSystem;
using Console.Core.PropertySystem;
using Console.Core.Utils;

namespace Console.EnvironmentVariables
{
    public class EnvInitializer : AExtensionInitializer
    {
        public override LoadOrder Order => LoadOrder.First;

        private class EnvExpander : AExpander
        {
            public override string Expand(string input)
            {
                return EnvironmentVariableManager.Expand(input);
            }
        }

        [Property("version.environmentvariables")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<EnvInitializer>();
            EnvironmentVariableManager.AddProvider(DefaultVariables.Instance);
            CommandAttributeUtils.AddCommands(typeof(EnvironmentVariableManager));
            AConsoleManager.ExpanderManager.AddExpander(new EnvExpander());
        }
    }
}