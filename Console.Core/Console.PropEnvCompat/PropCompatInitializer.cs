using System;
using System.Reflection;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;
using Console.EnvironmentVariables;

namespace Console.PropEnvCompat
{
    public class PropCompatInitializer : AExtensionInitializer
    {
        [Property("version.propenvcompat")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<PropCompatInitializer>();
            EnvironmentVariableManager.AddProvider(new PropertyVariableProvider());
        }
    }
}