using System.Collections.Generic;

namespace Console.ScriptSystem.Deblocker.Conditions
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
}