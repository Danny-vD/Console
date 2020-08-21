using System;

namespace Console.EnvironmentVariables
{
    /// <summary>
    /// Delegate Implementation of the Variable Provider.
    /// Allows Creation of Variable Providers without adding more classes.
    /// </summary>
    public class DelegateVariableProvider : VariableProvider
    {
        /// <summary>
        /// The Provider Func
        /// </summary>
        private readonly Func<string, string> Provider;
        /// <summary>
        /// The Function Name
        /// </summary>
        public override string FunctionName { get; }

        /// <summary>
        /// Returns the Value of the Provider
        /// </summary>
        /// <param name="parameter">Input Data</param>
        /// <returns>Value of the Provider</returns>
        public override string GetValue(string parameter) => Provider?.Invoke(parameter);

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="functionName">The Function Name that is used in the console: $funcName(data)</param>
        /// <param name="provider">Provider Func</param>
        public DelegateVariableProvider(string functionName, Func<string, string> provider)
        {
            FunctionName = functionName;
            Provider = provider;
        }
    }
}