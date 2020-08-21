using System.Collections.Generic;
using System.Linq;
using Console.Core;

namespace Console.ScriptSystem.Deblocker.Implementations
{
    /// <summary>
    /// ADeblocker Implementation with key "function"
    /// Implements Function Syntax with SequenceSystem as backend.
    /// </summary>
    public class FunctionDeblocker : DefaultDeblocker
    {

        /// <summary>
        /// The Key of the Deblocker that has to match the block command to be activated.
        /// </summary>
        public override string Key => "function";

        /// <summary>
        /// Returns the Deblocked Content of the Line
        /// </summary>
        /// <param name="line">Line to Deblock</param>
        /// <param name="begin">Lines that get prepended to the beginning of the file</param>
        /// <param name="end">Lines that get appended to the end of the file</param>
        /// <returns>List of Deblocked Content</returns>
        public override string[] Deblock(Line line, out List<string> begin, out List<string> end)
        {
            FunctionSignatureParser.FunctionSignature signature = FunctionSignatureParser.ParseFunctionSignature(line, out int sigStart, out int sigLength);
            Line l = new Line(SequenceSystem.SequenceAdd +
                              line.OriginalLine.Remove(sigStart, sigLength).Remove(0, Key.Length));


            string[] parts = l.CleanParts;
            if (DeblockerSettings.WriteDeblockLogs)
                ScriptSystemInitializer.Logger.Log("Deblocking Function: " + parts[1] + signature);
            List<string> s = base.Deblock(l, out begin, out end).ToList();
            if (parts.Length < 2)
            {
                ScriptSystemInitializer.Logger.LogWarning("Can not Deblock Line: " + l);
                begin = new List<string>();
                end = new List<string>();
                return base.Deblock(line, out begin, out end);
            }

            s.InsertRange(0, signature.ParameterNames.Select(x => $"{SequenceSystem.SequenceAddParameter} {parts[1]} {x}"));
            s.Insert(0, $"{SequenceSystem.SequenceCreate} {parts[1]} {SequenceSystem.SequenceCreateOverwrite}"); // Create after Delete
            //s.Insert(0, $"{SequenceSystem.SequenceDelete} {parts[1]}"); // Delete to make sure the name is free
            return s.ToArray();
        }
    }
}