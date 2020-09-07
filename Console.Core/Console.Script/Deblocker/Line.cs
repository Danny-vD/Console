﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

using Console.Core;
using Console.Core.CommandSystem;

namespace Console.Script.Deblocker
{
    /// <summary>
    /// Data Class that is used to Parse the Blocks into Lines.
    /// </summary>
    public class Line
    {

        /// <summary>
        /// The Original Line Backing Field
        /// </summary>
        private readonly string _originalLine;

        /// <summary>
        /// Blocks Backing Field
        /// </summary>
        private List<string[]> _blocks;

        /// <summary>
        /// Backing Field for CleanedLine
        /// </summary>
        private string _cleanedLine;

        /// <summary>
        /// Clean Parts Backing Field
        /// </summary>
        private string[] _cleanParts;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="line"></param>
        public Line(string line)
        {
            _originalLine = line;
        }

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="originalLine">Original Line</param>
        /// <param name="cleanLine">The Cleaned Line</param>
        /// <param name="blocks">The Blocks of this Line</param>
        public Line(string originalLine, string cleanLine, List<string[]> blocks) : this(originalLine)
        {
            _cleanedLine = cleanLine;
            _blocks = blocks;
            _cleanParts = CommandParser.ParseStringBlocks(_cleanedLine).Select(x => x.Trim()).ToArray();
        }

        /// <summary>
        /// The Original Line Backing Field
        /// </summary>
        public string OriginalLine => _originalLine;

        /// <summary>
        /// The Cleaned Line(Blocks are replaced with DeblockerSettings.GetKey keys)
        /// </summary>
        public string CleanedLine
        {
            get
            {
                if (_cleanedLine == null)
                {
                    Parse();
                }

                return _cleanedLine;
            }
        }

        /// <summary>
        /// The Collection of blocks in this Line
        /// </summary>
        public List<string[]> Blocks
        {
            get
            {
                if (_blocks == null)
                {
                    Parse();
                }

                return _blocks;
            }
        }

        /// <summary>
        /// The Clean Line but Correctly Split
        /// </summary>
        public string[] CleanParts
        {
            get
            {
                if (_cleanParts == null)
                {
                    Parse();
                }

                return _cleanParts;
            }
        }

        /// <summary>
        /// If True the Original line is already the final line.
        /// </summary>
        public bool IsAtomic => Blocks.Count == 0;

        /// <summary>
        /// Fills the Backed Properties with their parse results
        /// </summary>
        private void Parse()
        {
            _cleanedLine = GetCleanedLine(OriginalLine, out _blocks);
            _cleanParts = CommandParser.ParseStringBlocks(_cleanedLine).Select(x => x.Trim()).ToArray();
        }


        /// <summary>
        /// To String Implementation
        /// </summary>
        /// <returns>The Original String Line</returns>
        public override string ToString()
        {
            return OriginalLine;
        }


        /// <summary>
        /// Returns the Line with all blocks replaced with DeblockerSettings.GetKey keys.
        /// </summary>
        /// <param name="originalLine">The Original Line</param>
        /// <param name="blocks">The Blocks that were parsed</param>
        /// <returns>The Original Line with Replacement for the Blocks</returns>
        private string GetCleanedLine(string originalLine, out List<string[]> blocks)
        {
            StringBuilder line = new StringBuilder();
            blocks = new List<string[]>();
            for (int i = 0; i < originalLine.Length; i++)
            {
                if (originalLine[i] == DeblockerSettings.BlockBracketOpen)
                {
                    int close = ConsoleCoreConfig.FindClosing(
                                                              originalLine,
                                                              DeblockerSettings.BlockBracketOpen,
                                                              DeblockerSettings.BlockBracketClosed,
                                                              i
                                                             );
                    close = close == -1 ? originalLine.Length : close;
                    string l = originalLine.Substring(i + 1, close - i - 1).Trim();
                    List<string> subl = DeblockerCollection.Parse(l);
                    blocks.Add(subl.ToArray());
                    string ln = DeblockerSettings.GetKey(blocks.Count - 1);
                    if (line[line.Length - 1] != ' ')
                    {
                        ln = " " + ln;
                    }

                    i = close + 1;
                    if (i < originalLine.Length)
                    {
                        ln = ln + " ";
                    }

                    line.Append(ln);
                    continue;
                }

                line.Append(originalLine[i]);
            }

            string s = line.ToString();
            return s;
        }

    }
}