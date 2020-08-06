namespace Console.EnvironmentVariables
{
    public abstract class VariableProvider
    {
        public abstract string FunctionName { get; }
        public abstract string GetValue(string parameter);
    }
}