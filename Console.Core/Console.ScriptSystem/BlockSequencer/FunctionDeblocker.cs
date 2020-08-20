using System;
using System.Collections.Generic;
using System.Linq;
using Console.Core;
using Console.Core.CommandSystem;

namespace Console.ScriptSystem.BlockSequencer
{

    public class DefaultDeblocker : ADeblocker
    {
        public override string Key => "";

        /// <summary>
        /// Default Line Deblock Implementation
        /// </summary>
        /// <returns>Deblocked Content</returns>
        public override string[] Deblock(Line line, out List<string> begin, out List<string> end)
        {
            begin = new List<string>();
            end = new List<string>();
            if (line.Blocks.Count == 0) return new[] { line.CleanedLine };
            List<string> lines = new List<string>();
            int maxCount = line.Blocks.Max(strings => strings.Length);
            for (int i = 0; i < maxCount; i++)
            {
                string value = line.CleanedLine.ToString();
                for (int j = 0; j < line.Blocks.Count; j++)
                {
                    value = value.Replace(SequencerSettings.GetKey(j),
                        $"\"{CommandParser.Escape(line.Blocks[j][i % line.Blocks[j].Length], ConsoleCoreConfig.EscapeChar, ConsoleCoreConfig.EscapableChars)}\"");
                }
                lines.Add(value);
            }
            return lines.ToArray();
        }

    }

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
            //s.Insert(0, $"{SequenceSystem.SequenceDelete} {parts[1]}"); // Delete to make sure the name is free
            return s.ToArray();
        }
    }


    public class IfElseDeblocker : IfDeblocker
    {
        public override string Key => "ifelse";
    }
    public class IfElseIfDeblocker : IfDeblocker
    {
        public override string Key => "ifelseif";
    }

    /// <summary>
    /// ADeblocker Implementation with key "function"
    /// Implements Function Syntax with SequenceSystem as backend.
    /// </summary>
    public class IfDeblocker : DefaultDeblocker
    {

        /// <summary>
        /// The Key of the Deblocker that has to match the block command to be activated.
        /// </summary>
        public override string Key => "if";

        /// <summary>
        /// Returns the Deblocked Content of the Line
        /// </summary>
        /// <param name="line">Line to Deblock</param>
        /// <param name="begin">Lines that get prepended to the beginning of the file</param>
        /// <param name="end">Lines that get appended to the end of the file</param>
        /// <returns>List of Deblocked Content</returns>
        public override string[] Deblock(Line line, out List<string> begin, out List<string> end)
        {
            List<string> _begin=new List<string>();
            List<string> _end=new List<string>();
            begin = new List<string>();
            end = new List<string>();
            List<string> ret = new List<string>();
            string invocation = line.CleanedLine;
            for (int i = 0; i < line.Blocks.Count; i++)
            {
                string ifBlockSeq = SequencerSettings.GetBlockName();
                List<string> ifBlockContent = CreateBlock(ifBlockSeq, line.Blocks[i], out List<string> bbegin,
                    out List<string> bend);
                ret.AddRange(ifBlockContent);
                string rep = $"\"{SequenceSystem.SequenceRun} {ifBlockSeq}\"";
                //if (i != line.Blocks.Count - 1) rep = rep + ConsoleCoreConfig.CommandInputSeparator;
                invocation = invocation.Replace(SequencerSettings.GetKey(i), rep);
                _begin.InsertRange(0,bbegin);
                _end.AddRange(bend);
            }

            ret.Add(invocation);
            ret.InsertRange(0, _begin);
            ret.AddRange(_end);
            return ret.ToArray();
        }

        private List<string> CreateBlock(string name, string[] content, out List<string> begin, out List<string> end)
        {
            begin = new List<string>();
            end = new List<string>();
            List<string> ret = new List<string>();

            //ret.Add($"{SequenceSystem.SequenceDelete} {name}"); // Make sure the Block is Free
            ret.Add($"{SequenceSystem.SequenceCreate} {name} {SequenceSystem.SequenceCreateOverwrite}"); // Create

            foreach (string s in content)
            {
                if (string.IsNullOrEmpty(s)) continue;

                string item = $"{SequenceSystem.SequenceAdd} {name} \"{CommandParser.Escape(s, ConsoleCoreConfig.EscapeChar, ConsoleCoreConfig.EscapableChars)}\"";
                ret.Add(item);
            }

            end.Add($"{SequenceSystem.SequenceDelete} {name}"); // Delete in the End
            return ret;
        }
    }
}