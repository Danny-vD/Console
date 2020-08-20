using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Core;
using Console.Core.CommandSystem;

namespace Console.ScriptSystem.BlockSequencer
{

    public class Line
    {

        public bool IsAtomic => Blocks.Count == 0;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="line"></param>
        public Line(string line)
        {
            OriginalLine = line;
        }


        private static string GetCleanedLine(string originalLine, out List<string[]> blocks)
        {
            StringBuilder line = new StringBuilder();
            blocks = new List<string[]>();
            for (int i = 0; i < originalLine.Length; i++)
            {
                if (originalLine[i] == SequencerSettings.BlockBracketOpen)
                {
                    int close = ConsoleCoreConfig.FindClosing(originalLine, SequencerSettings.BlockBracketOpen,
                        SequencerSettings.BlockBracketClosed, i);
                    //int close = OriginalLine.IndexOf(SequencerSettings.BlockBracketClosed, i);
                    close = close == -1 ? originalLine.Length : close;
                    //List<string[]> sblocks = new List<string[]>();
                    string l = originalLine.Substring(i + 1, close - i - 1);
                    List<string> subl = Sequencer.Parse(l);
                    //string[] subl = originalLine.Substring(i + 1, close - i - 1)
                    //.Split(new[] {ConsoleCoreConfig.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                    //.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    //blocks.AddRange(sblocks);
                    blocks.Add(subl.ToArray());
                    string ln = SequencerSettings.GetKey(blocks.Count - 1);
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
        private string _cleanedLine;
        public string CleanedLine
        {
            get
            {
                if (_cleanedLine == null) Parse();
                return _cleanedLine;
            }
        }

        private List<string[]> _blocks;
        public List<string[]> Blocks
        {
            get
            {
                if (_blocks == null) Parse();
                return _blocks;
            }
        }

        private string[] _cleanParts;
        public string[] CleanParts
        {
            get
            {
                if (_cleanParts == null) Parse();
                return _cleanParts;
            }
        }


        private void Parse()
        {
            _cleanedLine = GetCleanedLine(OriginalLine, out _blocks);
            _cleanParts = CommandParser.ParseStringBlocks(_cleanedLine);
        }

        ///// <summary>
        ///// Default Line Deblock Implementation
        ///// </summary>
        ///// <returns>Deblocked Content</returns>
        //public string[] Deblock()
        //{
        //    if (Blocks.Count == 0) return new[] { CleanedLine };
        //    List<string> lines = new List<string>();
        //    int maxCount = Blocks.Max(strings => strings.Length);
        //    for (int i = 0; i < maxCount; i++)
        //    {
        //        string value = CleanedLine.ToString();
        //        for (int j = 0; j < Blocks.Count; j++)
        //        {
        //            value = value.Replace(SequencerSettings.GetKey(j), $"\"{Blocks[j][i % Blocks[j].Length].Replace("\"", "\\\"")}\"");
        //        }
        //        lines.Add(value);
        //    }
        //    return lines.ToArray();
        //}



        /// <summary>
        /// To String Implementation
        /// </summary>
        /// <returns>The Original String Line</returns>
        public override string ToString() => OriginalLine;
    }

    ///// <summary>
    ///// Line Script Containing the String Content of a Line
    ///// </summary>
    //public class Line
    //{
    //   
    //}
}