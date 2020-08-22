using System.Collections.Generic;

namespace Console.ScriptSystem.Deblocker.Conditions
{
    public class WhileDeblocker : IfDeblocker
    {
        public override string Key => "while";
        public override string[] Deblock(Line line, out List<string> begin, out List<string> end)
        {
            return Deblock(line, new string[0], out begin, out end);
        }
    }
}