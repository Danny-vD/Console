using System.Collections.Generic;
using System.Linq;
using Console.Core;

namespace Console.ScriptSystem.Deblocker.Implementations
{

    /// <summary>
    /// FunctionDeblocker Implementation with key "local-function"
    /// Creates a Sequence that is only usable in the same script as it is created.
    /// Implements Function Syntax with SequenceSystem as backend.
    /// Does Release the Resources of a Function when the Script Finished Executing.
    /// </summary>
    public class LocalFunctionDeblocker : DefaultDeblocker
    {
        /// <summary>
        /// The Key of the Deblocker that has to match the block command to be activated.
        /// </summary>
        public override string Key => "local-function";

        /// <summary>
        /// Returns the Deblocked Content of the Line
        /// </summary>
        /// <param name="line">Line to Deblock</param>
        /// <param name="begin">Lines that get prepended to the beginning of the file</param>
        /// <param name="end">Lines that get appended to the end of the file</param>
        /// <returns>List of Deblocked Content</returns>
        public override string[] Deblock(Line line, out List<string> begin, out List<string> end)
        {
            Line l = new Line(SequenceSystem.SequenceAdd + line.OriginalLine.Remove(0, Key.Length));
            List<string> s = base.Deblock(l, out begin, out end).ToList();
            string[] parts = l.CleanParts;
            if (parts.Length < 2)
            {
                AConsoleManager.Instance.LogWarning("Can not Deblock Line: " + l);
                begin = new List<string>();
                end = new List<string>();
                return base.Deblock(line, out begin, out end);
            }
            s.Insert(0, $"{SequenceSystem.SequenceCreate} {parts[1]} {SequenceSystem.SequenceCreateOverwrite}"); // Create after Delete
            end.Add($"{SequenceSystem.SequenceDelete} {parts[1]}"); // Delete to make sure the name is free
            return s.ToArray();
        }
    }
}