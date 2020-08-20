using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Console.Core;
using Console.Core.CommandSystem;

namespace Console.ScriptSystem.BlockSequencer
{
    /// <summary>
    /// Static Minimal Sequencer Parsing API
    /// </summary>
    public static class Sequencer
    {
        private static readonly DefaultDeblocker Default = new DefaultDeblocker();

        /// <summary>
        /// List of Deblockers
        /// </summary>
        private static readonly List<ADeblocker> Deblockers = new List<ADeblocker>();

        /// <summary>
        /// Adds a Deblocker to the System
        /// </summary>
        /// <param name="deblocker">Deblocker to Add</param>
        public static void AddDeblocker(ADeblocker deblocker)
        {
            Deblockers.Add(deblocker);
        }

        /// <summary>
        /// Returns the Parsed Lines as List
        /// </summary>
        /// <param name="content">Content to create the lines from</param>
        /// <param name="begin">Start index</param>
        /// <returns>List of Parsed Lines</returns>
        private static List<Line> ParseLines(string content, int begin = 0)
        {
            List<Line> ret = new List<Line>();
            int start = begin;
            int end = GetEndOfLine(content, start);
            while (end != content.Length)
            {
                string l = content.Substring(start, end - start).Trim();
                if (!string.IsNullOrWhiteSpace(l))
                {
                    ret.Add(new Line(l));
                }
                start = end + 1;
                end = GetEndOfLine(content, start);
                if (end == content.Length)
                {
                    string cline = content.Substring(start, end - start).Trim();
                    ret.Add(new Line(cline));
                }
            }
            return ret;
        }

        private static List<string> Parse(List<Line> lines)
        {
            List<string> ret = new List<string>();
            List<string> _end = new List<string>();
            foreach (Line line in lines)
            {
                if (line.OriginalLine == "}") continue;

                if (line.IsAtomic)
                {
                    ret.Add(line.OriginalLine);
                    continue;
                }
                ADeblocker db = Deblockers.FirstOrDefault(x => line.CleanParts.First() == x.Key) ?? Default;

                List<Line> blocks = db.Deblock(line, out List<string> begin, out List<string> end).Select(x => new Line(x)).ToList();
                List<string> b = Parse(blocks);
                ret.AddRange(b);
                //ret.InsertRange(0, begin);
                _end.AddRange(end);
            }
            ret.AddRange(_end);
            
            return ret;
        }

        /// <summary>
        /// Returns the Parsed and Deblocked Content of the File
        /// </summary>
        /// <param name="content">File Content</param>
        /// <returns>Parsed and Deblocked Content</returns>
        public static List<string> Parse(string content)
        {
            List<Line> lines = ParseLines(content);
            return Parse(lines);
        }

        /// <summary>
        /// returns the index of either the next new line Skipping all script blocks.
        /// </summary>
        /// <param name="content">File Content</param>
        /// <param name="start">Start index</param>
        /// <returns>Index of the next newline or end of file.</returns>
        private static int GetEndOfLine(string content, int start)
        {
            for (int i = start; i < content.Length; i++)
            {
                if (content[i] == SequencerSettings.BlockBracketOpen)
                {
                    int closeIdx = ConsoleCoreConfig.FindClosing(content, SequencerSettings.BlockBracketOpen,
                        SequencerSettings.BlockBracketClosed, i + 1, 1);
                    string s = content.Substring(i, closeIdx - i + 1);
                    //int closeIdx = content.IndexOf(SequencerSettings.BlockBracketClosed, i + 1);
                    i = closeIdx;
                }
                else if (content[i] == ConsoleCoreConfig.NewLine)
                {
                    return i;
                }
            }
            return content.Length;
        }



    }
}