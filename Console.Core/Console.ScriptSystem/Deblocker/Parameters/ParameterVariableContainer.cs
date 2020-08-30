using Console.EnvironmentVariables;


/// <summary>
/// Implements the Parameter Parsing and Provider of Parameterized Sequences
/// </summary>
namespace Console.ScriptSystem.Deblocker.Parameters
{
    /// <summary>
    /// Implements the Parameter Provider for Parameterized Sequences.
    /// </summary>
    public class ParameterVariableContainer : VariableProvider
    {
        /// <summary>
        /// The Provider Name
        /// </summary>
        public override string FunctionName => "param";

        /// <summary>
        /// Returns the Parameter specified from the Current ParameterCollection
        /// </summary>
        /// <param name="parameter">Parameter Name</param>
        /// <returns>Value of the Parameter</returns>
        public override string GetValue(string parameter)
        {
            return ParameterCollection.GetParameter(parameter);
        }
    }
}