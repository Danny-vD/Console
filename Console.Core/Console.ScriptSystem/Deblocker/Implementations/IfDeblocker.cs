using System.Collections.Generic;
using Console.Core;
using Console.Core.CommandSystem;

namespace Console.ScriptSystem.Deblocker.Implementations
{
    /// <summary>
    /// ADeblocker Implementation with key "if"
    /// Implements if Syntax with SequenceSystem / Evaluator Extension as backend.
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
            return Deblock(line, new string[0], out begin, out end);
        }

        protected string[] Deblock(Line line, string[] parameters, out List<string> begin, out List<string> end)
        {
            if (DeblockerSettings.WriteDeblockLogs)
                ScriptSystemInitializer.Logger.Log($"Deblocking {Key}: " + line.CleanedLine);
            List<string> _begin = new List<string>();
            List<string> _end = new List<string>();
            begin = new List<string>();
            end = new List<string>();
            List<string> ret = new List<string>();
            string invocation = line.CleanedLine;
            
            for (int i = 0; i < line.Blocks.Count; i++)
            {
                string ifBlockSeq = DeblockerSettings.GetBlockName();
                List<string> ifBlockContent = CreateBlock(ifBlockSeq, line.Blocks[i], parameters, out List<string> bbegin,
                    out List<string> bend);
                ret.AddRange(ifBlockContent);
                string rep = $"\"{SequenceSystem.SequenceRun} {ifBlockSeq}\"";
                //if (i != line.Blocks.Count - 1) rep = rep + ConsoleCoreConfig.CommandInputSeparator;
                invocation = invocation.Replace(DeblockerSettings.GetKey(i), rep);
                _begin.InsertRange(0, bbegin);
                _end.AddRange(bend);
            }

            ret.Add(invocation);
            ret.InsertRange(0, _begin);
            ret.AddRange(_end);
            return ret.ToArray();
        }

        protected List<string> CreateBlock(string name, string[] content, string[] parameters, out List<string> begin, out List<string> end)
        {
            begin = new List<string>();
            end = new List<string>();
            List<string> ret = new List<string>();

            //ret.Add($"{SequenceSystem.SequenceDelete} {name}"); // Make sure the Block is Free
            ret.Add($"{SequenceSystem.SequenceCreate} {name} {SequenceSystem.SequenceCreateOverwrite}"); // Create
            for (int i = 0; i < parameters.Length; i++)
            {
                ret.Add($"{SequenceSystem.SequenceAddParameter} {name} {parameters[i]}");
            }

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