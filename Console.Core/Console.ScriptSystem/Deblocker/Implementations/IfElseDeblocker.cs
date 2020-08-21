using System.Collections.Generic;
using System.Linq;

namespace Console.ScriptSystem.Deblocker.Implementations
{

    public class ForAllDeblocker : IfDeblocker
    {
        public override string Key => "for-all";

        public override string[] Deblock(Line line, out List<string> begin, out List<string> end)
        { 
            string[] ret= Deblock(line, new[] {"item"}, out begin, out end);
            return ret;
        }
    }

    /// <summary>
    /// ADeblocker Implementation with key "ifelse"
    /// Implements if Syntax with SequenceSystem / Evaluator Extension as backend.
    /// </summary>
    public class IfElseDeblocker : IfDeblocker
    {
        /// <summary>
        /// The Key of the Deblocker that has to match the block command to be activated.
        /// </summary>
        public override string Key => "ifelse";
    }
}