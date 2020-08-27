using System.Collections.Generic;
using System.Linq;
using Console.Core;
using Console.Core.CommandSystem;

namespace Console.ScriptSystem.Deblocker.Implementations
{
    /// <summary>
    /// Default ADeblocker Implementation.
    /// Does Parse the Blocks and Creates as many lines of the command as the longest block.
    /// Shorter Blocks get Repeated with Modulo
    /// </summary>
    public class DefaultDeblocker : ADeblocker
    {
        /// <summary>
        /// The Key of the Deblocker that has to match the block command to be activated.
        /// </summary>
        public override string Key => "";

        /// <summary>
        /// Returns the Deblocked Content of the Line
        /// </summary>
        /// <param name="line">Line to Deblock</param>
        /// <param name="begin">Lines that get prepended to the beginning of the file</param>
        /// <param name="end">Lines that get appended to the end of the file</param>
        /// <returns>List of Deblocked Content</returns>
        public override string[] Deblock(Line line, out List<string> begin, out List<string> end)
        {
            begin = new List<string>();
            end = new List<string>();
            if (line.Blocks.Count == 0)
            {
                DeblockerSettings.LogVerbose($"Line: \"{line.CleanedLine}\" has no Blocks.");
                return new[] {line.CleanedLine};
            }
            List<string> lines = new List<string>();
            int maxCount = line.Blocks.Max(strings => strings.Length);
            DeblockerSettings.LogVerbose($"Line Block Count: {line.Blocks.Count}, Block Length: {maxCount}");
            for (int i = 0; i < maxCount; i++)
            {
                string value = line.CleanedLine;
                for (int j = 0; j < line.Blocks.Count; j++)
                {
                    value = value.Replace(DeblockerSettings.GetKey(j),
                        $"\"{CommandParser.Escape(line.Blocks[j][i % line.Blocks[j].Length], ConsoleCoreConfig.EscapeChar, ConsoleCoreConfig.EscapableChars)}\"");
                }
                lines.Add(value);
                DeblockerSettings.LogVerbose($"Deblocked Line: \"{value}\"");
            }
            return lines.ToArray();
        }
    }
}