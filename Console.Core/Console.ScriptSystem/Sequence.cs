using System.Collections.Generic;
using Console.ScriptSystem.Deblocker.Functions.Internal;
using Console.ScriptSystem.Deblocker.Implementations;

namespace Console.ScriptSystem
{
    internal class Sequence
    {
        public readonly FunctionSignature Signature;
        public readonly List<string> Lines;

        public Sequence()
        {
            Lines = new List<string>();
            Signature = new FunctionSignature("");
        }
    }
}