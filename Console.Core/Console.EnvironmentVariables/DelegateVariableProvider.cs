using System;
using System.Reflection;
using Console.Core.Console;

namespace Console.EnvironmentVariables
{
    public class DelegateVariableProvider : VariableProvider
    {
        private readonly Func<string, string> Provider;
        public override string FunctionName { get; }

        public override string GetValue(string parameter) => Provider?.Invoke(parameter);

        public DelegateVariableProvider(string functionName, Func<string, string> provider)
        {
            FunctionName = functionName;
            Provider = provider;
        }
    }
}