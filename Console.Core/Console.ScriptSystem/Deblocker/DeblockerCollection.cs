﻿using System;
using System.Collections.Generic;
using System.Linq;
using Console.Core;
using Console.ScriptSystem.Deblocker.Implementations;

/// <summary>
/// Deblocker System which is used when loading Scripts or Sequences from File.
/// </summary>
namespace Console.ScriptSystem.Deblocker
{
    /// <summary>
    /// Static Minimal DeblockerCollection Parsing API
    /// </summary>
    public static class DeblockerCollection
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
            int end;
            do
            {
                end = GetEndOfLine(content, start);
                string l = content.Substring(start, end - start).Trim();
                if (!string.IsNullOrWhiteSpace(l))
                {
                    ret.Add(new Line(l));
                }
                start = end + 1;
                //if (end == content.Length)
                //{
                //    string cline = content.Substring(start, end - start).Trim();
                //    ret.Add(new Line(cline));
                //}
            } while (end != content.Length);
            return ret;
        }

        private static List<string> Parse(List<Line> lines)
        {
            List<string> ret = new List<string>();
            List<string> _end = new List<string>();
            foreach (Line line in lines)
            {
                if (line.OriginalLine == "}")
                {
                    continue;
                }

                if (line.IsAtomic)
                {
                    ret.Add(line.OriginalLine);
                    continue;
                }
                ADeblocker db = Deblockers.FirstOrDefault(x => line.CleanParts.First() == x.Key) ?? Default;

                List<Line> blocks = db.Deblock(line, out List<string> begin, out List<string> end)
                    .Select(x => new Line(x)).ToList();
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
            string clean = RemoveComments(content, DeblockerSettings.CommentPrefix, ConsoleCoreConfig.NewLine.ToString(), 0);
            clean = RemoveComments(clean, DeblockerSettings.CommentMultiPrefix, DeblockerSettings.CommentMultiPostfix, DeblockerSettings.CommentMultiPostfix.Length);
            List<Line> lines = ParseLines(clean);
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
                if (content[i] == DeblockerSettings.BlockBracketOpen)
                {
                    int closeIdx = ConsoleCoreConfig.FindClosing(content, DeblockerSettings.BlockBracketOpen,
                        DeblockerSettings.BlockBracketClosed, i + 1, 1);
                    string s = content.Substring(i, closeIdx - i + 1);
                    i = closeIdx;
                }
                else if (content[i] == ConsoleCoreConfig.NewLine)
                {
                    return i;
                }
            }
            return content.Length;
        }

        private static string RemoveComments(string content, string pre, string post, int endOff)
        {
            string ret = content;
            while (true)
            {
                int commentIdx = ret.IndexOf(pre, StringComparison.InvariantCulture);
                if (commentIdx == -1) return ret;
                int endIdx = ret.IndexOf(post, commentIdx, StringComparison.InvariantCulture);
                endIdx = endIdx == -1 ? ret.Length : endIdx + endOff;
                ret = ret.Remove(commentIdx, endIdx - commentIdx);
            }
        }
    }
}