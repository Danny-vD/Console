using System.Collections.Generic;

namespace Console.ScriptSystem.Deblocker.Conditions
{
    /// <summary>
    /// Implements the deblocking for the "for-all" command.
    /// </summary>
    public class ForAllDeblocker : IfDeblocker
    {
        /// <summary>
        /// Deblocker Key
        /// </summary>
        public override string Key => "for-all";

        /// <summary>
        /// Deblocks the for-all command.
        /// </summary>
        /// <param name="line">The Command</param>
        /// <param name="begin">Lines that should be prepended before the line deblock</param>
        /// <param name="end">Lines that should be appended after the line deblock</param>
        /// <returns>The Deblocked Line</returns>
        public override string[] Deblock(Line line, out List<string> begin, out List<string> end)
        { 
            string[] ret= Deblock(line, new[] {"item"}, out begin, out end);
            return ret;
        }
    }
}