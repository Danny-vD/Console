using System.Collections.Generic;
using System.Linq;

using Console.Script.Deblocker.Functions.Internal;
using Console.Script.Deblocker.Implementations;

namespace Console.Script.Deblocker.Functions
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
            FunctionSignature signature = FunctionSignature.Parse(line, out int sigStart, out int sigLength);
            string newOrig = SequenceSystem.SequenceAdd +
                             line.OriginalLine.Remove(sigStart, sigLength).Remove(0, Key.Length);
            string cleanLine = SequenceSystem.SequenceAdd +
                               line.CleanedLine.Remove(sigStart, sigLength).Remove(0, Key.Length);
            Line l = new Line(newOrig, cleanLine, line.Blocks);


            string[] parts = l.CleanParts;
            DeblockerSettings.Logger.Log("Deblocking Function: " + parts[1] + signature);
            List<string> s = base.Deblock(l, out begin, out end).ToList();
            if (parts.Length < 2)
            {
                DeblockerSettings.Logger.LogWarning("Can not Deblock Line: " + l);
                begin = new List<string>();
                end = new List<string>();
                return base.Deblock(line, out begin, out end);
            }

            string ps = $"{SequenceSystem.SequenceAddParameter} {parts[1]} ";
            for (int i = 0; i < signature.ParameterNames.Count; i++)
            {
                ps += signature.ParameterNames[i] + ";";
            }

            if (signature.ParameterNames.Count > 0)
            {
                s.Insert(0, ps);
            }

            //s.InsertRange(0, signature.ParameterNames.Select(x => $"{SequenceSystem.SequenceAddParameter} {parts[1]} {x}"));
            s.Insert(
                     0,
                     $"{SequenceSystem.SequenceCreate} {parts[1]} {SequenceSystem.SequenceCreateOverwrite}"
                    ); // Create

            return s.ToArray();
        }

    }
}