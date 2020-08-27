using System;

namespace Console.Evaluator.Core
{
    /// <summary>
    /// Gets Thrown when the Evaluator does not find a variable name.
    /// If the Evaluator.RaiseVariableNotFoundException is set to false this exception will not be thrown
    /// </summary>
    public class VariableNotFoundException : Exception
    {
        /// <summary>
        /// The Variable Name that was not found
        /// </summary>
        public readonly string VariableName;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="variableName">The Variable Name that was not found</param>
        /// <param name="innerException">The Inner Exception</param>
        public VariableNotFoundException(string variableName, Exception innerException = null) : base(
            variableName + " was not found", null)
        {
            VariableName = variableName;
        }
    }
}