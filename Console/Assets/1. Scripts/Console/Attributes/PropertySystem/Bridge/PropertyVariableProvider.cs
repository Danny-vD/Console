using Assets.Console.EnvironmentVariables;

namespace Console.Attributes.PropertySystem.Helper
{
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