using System.Collections.Generic;

using Console.Script.Deblocker.Functions.Internal;

namespace Console.Script
{
    /// <summary>
    /// The Internal Representation of a Sequence
    /// </summary>
    internal class Sequence
    {

        /// <summary>
        /// The Sequence Content
        /// </summary>
        public readonly List<string> Lines;

        /// <summary>
        /// The Sequence Signature
        /// </summary>
        public readonly FunctionSignature Signature;

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