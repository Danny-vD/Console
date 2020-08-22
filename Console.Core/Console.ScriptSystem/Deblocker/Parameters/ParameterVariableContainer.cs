using Console.EnvironmentVariables;

namespace Console.ScriptSystem.Deblocker.Parameters
{
    public class ParameterVariableContainer : VariableProvider
    {
        public override string FunctionName => "param";
        public override string GetValue(string parameter) => ParameterCollection.GetParameter(parameter);
    }
}