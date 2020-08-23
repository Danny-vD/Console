using System;

namespace Console.Evaluator.Core
{
    /// <summary>
    /// Gets thrown when the Evaluator encounters an Error
    /// </summary>
    public class ParserException : Exception
    {
        /// <summary>
        /// The Formula that failed to parse
        /// </summary>
        public readonly string Formula;
        /// <summary>
        /// The Index in the Formula where the parser went wrong
        /// </summary>
        public readonly int Pos;

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="str">The Error Message</param>
        /// <param name="formula">The Formula that failed to parse</param>
        /// <param name="pos">The Index in the Formula where the parser went wrong</param>
        internal ParserException(string str, string formula, int pos) : base(str)
        {
            this.Formula = formula;
            this.Pos = pos;
        }
    }
}