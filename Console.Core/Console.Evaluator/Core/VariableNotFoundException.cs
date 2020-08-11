using System;

namespace Console.Evaluator.Core
{
    public class VariableNotFoundException : Exception
    {
        public readonly string VariableName;

        public VariableNotFoundException(string variableName, Exception innerException = null) : base(variableName + " was not found", null)
        {
            VariableName = variableName;
        }
    }
}