using System;
using System.Collections.Generic;
using System.Linq;
using Console.Core;

namespace Console.ScriptSystem.BlockSequencer
{
    public class LocalFunctionDeblocker : DefaultDeblocker
    {
        public override string Key => "local-function";
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