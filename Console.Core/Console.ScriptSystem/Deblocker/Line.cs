using System.Collections.Generic;
using System.Text;
using Console.Core;
using Console.Core.CommandSystem;

namespace Console.ScriptSystem.Deblocker
{

    /// <summary>
    /// Data Class that is used to Parse the Blocks into Lines.
    /// </summary>
    public class Line
    {

        /// <summary>
        /// If True the Original line is already the final line.
        /// </summary>
        public bool IsAtomic => Blocks.Count == 0;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="line"></param>
        public Line(string line)
        {
            OriginalLine = line;
        }

        /// <summary>
        /// Returns the Line with all blocks replaced with DeblockerSettings.GetKey keys.
        /// </summary>
        /// <param name="originalLine">The Original Line</param>
        /// <param name="blocks">The Blocks that were parsed</param>
        /// <returns>The Original Line with Replacement for the Blocks</returns>
        private static string GetCleanedLine(string originalLine, out List<string[]> blocks)
        {
            StringBuilder line = new StringBuilder();
            blocks = new List<string[]>();
            for (int i = 0; i < originalLine.Length; i++)
            {
                if (originalLine[i] == DeblockerSettings.BlockBracketOpen)
                {
                    int close = ConsoleCoreConfig.FindClosing(originalLine, DeblockerSettings.BlockBracketOpen,
                        DeblockerSettings.BlockBracketClosed, i);
                    //int close = OriginalLine.IndexOf(DeblockerSettings.BlockBracketClosed, i);
                    close = close == -1 ? originalLine.Length : close;
                    string l = originalLine.Substring(i + 1, close - i - 1);
                    List<string> subl = DeblockerCollection.Parse(l);
                    blocks.Add(subl.ToArray());
                    string ln = DeblockerSettings.GetKey(blocks.Count - 1);
                    if (line[line.Length - 1] != ' ')
                        ln = " " + ln;
                    i = close + 1;
                    if (i < originalLine.Length)
                        ln = ln + " ";
                    line.Append(ln);
                    continue;
                }
                line.Append(originalLine[i]);
            }
            return line.ToString();
        }

        /// <summary>
        /// The Original Line
        /// </summary>
        public readonly string OriginalLine;

        /// <summary>
        /// Backing Field for CleanedLine
        /// </summary>
        private string _cleanedLine;

        /// <summary>
        /// The Cleaned Line(Blocks are replaced with DeblockerSettings.GetKey keys)
        /// </summary>
        public string CleanedLine
        {
            get
            {
                if (_cleanedLine == null) Parse();
                return _cleanedLine;
            }
        }

        /// <summary>
        /// Blocks Backing Field
        /// </summary>
        private List<string[]> _blocks;

        /// <summary>
        /// The Collection of blocks in this Line
        /// </summary>
        public List<string[]> Blocks
        {
            get
            {
                if (_blocks == null) Parse();
                return _blocks;
            }
        }

        /// <summary>
        /// Clean Parts Backing Field
        /// </summary>
        private string[] _cleanParts;

        /// <summary>
        /// The Clean Line but Correctly Split
        /// </summary>
        public string[] CleanParts
        {
            get
            {
                if (_cleanParts == null) Parse();
                return _cleanParts;
            }
        }

        /// <summary>
        /// Fills the Backed Properties with their parse results
        /// </summary>
        private void Parse()
        {
            _cleanedLine = GetCleanedLine(OriginalLine, out _blocks);
            _cleanParts = CommandParser.ParseStringBlocks(_cleanedLine);
        }


        /// <summary>
        /// To String Implementation
        /// </summary>
        /// <returns>The Original String Line</returns>
        public override string ToString() => OriginalLine;
    }
}