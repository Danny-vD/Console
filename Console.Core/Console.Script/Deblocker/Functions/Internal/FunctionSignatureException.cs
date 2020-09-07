using System;

namespace Console.Script.Deblocker.Functions.Internal
{
    /// <summary>
    /// A FunctionSignatureException gets thrown when the FunctionSignature is not able to be parsed.
    /// </summary>
    public class FunctionSignatureException : Exception
    {

        /// <summary>
        /// Public Constructor.
        /// </summary>
        /// <param name="message">Error Message.</param>
        public FunctionSignatureException(string message) : base(message)
        {
        }

    }
}