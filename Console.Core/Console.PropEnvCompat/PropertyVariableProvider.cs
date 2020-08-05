
using Console.Core;
using Console.Core.Attributes.PropertySystem.Helper;
using Console.EnvironmentVariables;

namespace Console.PropEnvCompat
{
    public class PropCompatInitializer : AExtensionInitializer
    {
        public override void Initialize()
        {
            EnvironmentVariableManager.AddProvider(new PropertyVariableProvider());
        }
    }

    public class PropertyVariableProvider : VariableProvider
    {
        public override string FunctionName => "props";
        public override string GetValue(string parameter)
        {
            if (ConsolePropertyAttributeUtils.TryGetValue(parameter, out object ret))
            {
                return ret.ToString();
            }
            return "NO_VALUE";
        }
    }
}