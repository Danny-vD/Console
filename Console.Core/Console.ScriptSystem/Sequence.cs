using System.Collections.Generic;
using Console.ScriptSystem.Deblocker.Functions.Internal;

namespace Console.ScriptSystem
{
    /// <summary>
    /// The Internal Representation of a Sequence
    /// </summary>
    internal class Sequence
    {
        /// <summary>
        /// The Sequence Signature
        /// </summary>
        public readonly FunctionSignature Signature;
        /// <summary>
        /// The Sequence Content
        /// </summary>
        public readonly List<string> Lines;
        /// <summary>
        /// Public Constructor
        /// </summary>
        public Sequence()
        {
            Lines = new List<string>();
            Signature = new FunctionSignature("");
        }
    }
}