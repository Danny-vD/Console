namespace Console.Vars
{
    /// <summary>
    /// Abstract Variable Provider.
    /// Can be Inherited to create a VariableProvider for the EnvironmentVariable System.
    /// </summary>
    public abstract class VariableProvider
    {

        /// <summary>
        /// The Function Name
        /// </summary>
        public abstract string FunctionName { get; }

        /// <summary>
        /// Returns the Value of the Provider
        /// </summary>
        /// <param name="parameter">Input Data</param>
        /// <returns>Value of the Provider</returns>
        public abstract string GetValue(string parameter);

    }
}