using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Console.Core.CommandSystem.Commands;


/// <summary>
/// Contains the Command Builder / AutoFillProviders Implementations of the Console Library.
/// </summary>
namespace Console.Core.CommandSystem.Builder
{
    /// <summary>
    /// Class Used to Build Commands with AutoFill Capabilites
    /// </summary>
    public class CommandBuilder : ICommandBuilder
    {

        private static readonly CommandBuilder instance = new CommandBuilder();

        private readonly StringBuilder builder = new StringBuilder();
        private readonly List<string> history = new List<string>();
        private int HistoryIndex;

        /// <summary>
        /// Is true if the Command was Built
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// The Cursor Position of the Builder
        /// </summary>
        public int Cursor { get; private set; }

        /// <summary>
        /// Handles the Input From the Console
        /// </summary>
        /// <param name="info">Key Info</param>
        public void Input(ConsoleKeyInfo info)
        {
            if (IsCompleted)
            {
                return;
            }

            switch (info.Key)
            {
                case ConsoleKey.Enter:
                    AddToHistory();
                    IsCompleted = true;
                    break;
                case ConsoleKey.Backspace:
                    Back();
                    break;
                case ConsoleKey.Delete:
                    Delete();
                    break;
                case ConsoleKey.Escape:
                    Clear();
                    break;
                case ConsoleKey.LeftArrow:
                    MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    MoveRight();
                    break;
                case ConsoleKey.Tab:
                    AutoFill();
                    break;
                case ConsoleKey.UpArrow:
                    LastHistoryEntry();
                    break;
                case ConsoleKey.DownArrow:
                    NextHistoryEntry();
                    break;
                default:
                    Append(info);
                    break;
            }
        }

        /// <summary>
        /// Returns the Command String
        /// </summary>
        /// <returns>Command String</returns>
        public override string ToString()
        {
            return builder.ToString();
        }


        private static string GetPrefix()
        {
            string bdir = AppDomain.CurrentDomain.BaseDirectory;
            bdir = bdir.Remove(bdir.Length - 1, 1);
            string cdir = Directory.GetCurrentDirectory();
            string p =
                $"{cdir.Replace(bdir, "").Replace('\\', '/')}/>";
            return p;
        }

        /// <summary>
        /// This is a Blocking Call.
        /// Builds a Command based on the Input.
        /// </summary>
        /// <param name="input">Input Abstraction</param>
        /// <param name="prefix">If true will write the prefix infront of the command</param>
        /// <returns>Built Command</returns>
        public static string BuildCommand(ICommandBuilderInput input, bool prefix = true)
        {
            string pre = prefix ? GetPrefix() : "";
            input.Write(pre);
            while (!instance.IsCompleted && !input.Abort)
            {
                ConsoleKeyInfo ki = input.ReadKey();
                instance.Input(ki);

                input.ResetLine();

                input.Write(pre);
                input.Write(instance.ToString());
                input.SetCursorPosition(pre.Length + instance.Cursor);
            }

            input.EndLine();
            string s = instance.ToString();
            instance.Clear();
            instance.HistoryIndex = instance.history.Count;
            return s;
        }


        private string GetPart(int index)
        {
            (int start, int length) = GetPartBounds(index);
            string s = builder.ToString(start, length);
            return s;
        }

        private (int, int) GetPartBounds(int index)
        {
            int parts = 0;
            bool inQuotes = false;
            int last = 0;
            for (int i = 0; i < builder.Length; i++)
            {
                if (builder[i] == ' ')
                {
                    if (!inQuotes)
                    {
                        parts++;
                        if (parts == index + 1)
                        {
                            return (last, i - last);
                        }

                        last = i + 1;
                    }
                }

                if (builder[i] == ConsoleCoreConfig.StringChar)
                {
                    if (!CommandParser.IsEscaped(builder.ToString(), i))
                    {
                        if (inQuotes)
                        {
                            parts++;
                            last = i;
                        }

                        inQuotes = !inQuotes;
                    }
                }
            }

            return (last, builder.Length - last);
        }

        private int GetCurrentParameterPartIndex()
        {
            int parts = 0;
            bool inQuotes = false;
            for (int i = 0; i < builder.Length; i++)
            {
                if (i == Cursor)
                {
                    return parts;
                }

                if (builder[i] == ' ')
                {
                    if (!inQuotes)
                    {
                        parts++;
                    }
                }

                if (builder[i] == ConsoleCoreConfig.StringChar)
                {
                    if (!CommandParser.IsEscaped(builder.ToString(), i))
                    {
                        if (inQuotes)
                        {
                            parts++;
                        }

                        inQuotes = !inQuotes;
                    }
                }
            }

            return parts;
        }

        private string[] GetPossibleAutoFills(
            AutoFillProvider[] providers, AbstractCommand[] commands, int paramIndex, string paramStart)
        {
            List<string> fills = new List<string>();
            foreach (AutoFillProvider autoFillProvider in providers)
            {
                foreach (AbstractCommand abstractCommand in commands)
                {
                    string[] f = autoFillProvider.GetAutoFills(abstractCommand, paramIndex, paramStart);
                    foreach (string fill in f)
                    {
                        if (!fills.Contains(fill))
                        {
                            fills.Add(fill);
                        }
                    }
                }
            }

            return fills.ToArray();
        }

        private AutoFillProvider[] GetPossibleProviders(AbstractCommand[] commands, int paramIndex)
        {
            List<AutoFillProvider> ret = new List<AutoFillProvider>();
            for (int i = 0; i < commands.Length; i++)
            {
                for (int j = 0; j < Providers.Count; j++)
                {
                    if (ret.Contains(Providers[j]))
                    {
                        continue;
                    }

                    if (Providers[j].CanFill(commands[i], paramIndex))
                    {
                        ret.Add(Providers[j]);
                    }
                }
            }

            return ret.ToArray();
        }

        private AbstractCommand[] GetPossibleCommands()
        {
            string cmd = GetPart(0);
            AbstractCommand[] cmds =
                CommandManager.AllCommands.Where(x => x.Identity.FilteredContainsName(cmd, true)).ToArray();
            return cmds;
        }

        #region Provider Commands

        /// <summary>
        /// The AutoFillProvider List
        /// </summary>
        private static readonly List<AutoFillProvider> Providers = new List<AutoFillProvider>();

        /// <summary>
        /// Adds the Specified AutoFill Provider to the CommandBuilder System
        /// </summary>
        /// <param name="provider">The Provider to be Added</param>
        public static void AddProvider(AutoFillProvider provider)
        {
            if (Providers.Contains(provider))
            {
                return;
            }

            Providers.Add(provider);
        }

        /// <summary>
        /// Adds the Specified AutoFill Providers to the CommandBuilder System
        /// </summary>
        /// <param name="providers">The Providers to be Added</param>
        public static void AddProvider(IEnumerable<AutoFillProvider> providers)
        {
            foreach (AutoFillProvider autoFillProvider in providers)
            {
                AddProvider(autoFillProvider);
            }
        }

        #endregion

        #region Input Functionality

        private void MoveLeft()
        {
            if (Cursor > 0)
            {
                Cursor--;
            }
        }

        private void MoveRight()
        {
            if (Cursor < builder.Length)
            {
                Cursor++;
            }
        }

        private void Back()
        {
            if (Cursor > 0)
            {
                Cursor--;
                builder.Remove(Cursor, 1);
            }
        }

        private void Delete()
        {
            if (Cursor < builder.Length)
            {
                builder.Remove(Cursor, 1);
            }
        }

        private void LastHistoryEntry()
        {
            if (HistoryIndex == 0)
            {
                return;
            }

            HistoryIndex--;
            WriteHistory(HistoryIndex);
        }

        private void Set(string value)
        {
            builder.Clear();
            builder.Append(value);
            Cursor = builder.Length;
        }

        private void WriteHistory(int index)
        {
            Set(history[HistoryIndex]);
        }

        private void NextHistoryEntry()
        {
            if (HistoryIndex == history.Count)
            {
                return;
            }

            if (HistoryIndex == history.Count - 1)
            {
                Set("");
                HistoryIndex++;
                return;
            }

            HistoryIndex++;
            WriteHistory(HistoryIndex);
        }

        private void Append(ConsoleKeyInfo info)
        {
            builder.Insert(Cursor, info.KeyChar);
            Cursor++;
        }

        private void ReplacePart(int part, string text)
        {
            (int start, int length) = GetPartBounds(part);
            builder.Remove(start, length);
            builder.Insert(start, text);
            Cursor = start + text.Length;
        }

        private void AutoFill()
        {
            AbstractCommand[] cmds = GetPossibleCommands();
            int partIdx = GetCurrentParameterPartIndex();
            string searchPart = GetPart(partIdx);
            AutoFillProvider[] providers = GetPossibleProviders(cmds, partIdx);
            string[] fills = GetPossibleAutoFills(providers, cmds, partIdx, searchPart);
            if (fills.Length == 0)
            {
                return;
            }

            if (fills.Length == 1)
            {
                ReplacePart(partIdx, fills.First());
            }
            else if (!ReplaceUntilAmbiguous(partIdx, searchPart, fills))
            {
                PrintOptions(fills);
            }
        }

        private bool ReplaceUntilAmbiguous(int partIdx, string input, string[] fills)
        {
            string str = GetLongestCommonSubstring(fills);
            if (str == input) return false;
            ReplacePart(partIdx, str);
            return true;
        }

        private static string GetLongestCommonSubstring(params string[] strings)
        {
            HashSet<string> commonSubstrings = new HashSet<string>(GetSubstrings(strings[0]));
            foreach (string str in strings.Skip(1))
            {
                commonSubstrings.IntersectWith(GetSubstrings(str));
                if (commonSubstrings.Count == 0)
                    return string.Empty;
            }

            return commonSubstrings.OrderByDescending(s => s.Length).DefaultIfEmpty(string.Empty).First();
        }

        private static IEnumerable<string> GetSubstrings(string str)
        {
            for (int cc = 1; cc <= str.Length; cc++)
            {
                yield return str.Substring(0, cc);
            }
        }

        /// <summary>
        /// Clears the Command Builder
        /// </summary>
        public void Clear()
        {
            IsCompleted = false;
            builder.Clear();
            Cursor = 0;
        }


        private void AddToHistory()
        {
            history.Add(ToString());
            HistoryIndex++;
        }

        private void PrintOptions(string[] options)
        {
            StringBuilder sb = new StringBuilder("\nPossible Options:\n");
            for (int i = 0; i < options.Length; i++)
            {
                sb.AppendLine('\t' + options[i]);
            }

            ConsoleCoreConfig.CoreLogger.Log(sb.ToString());
        }

        #endregion

    }
}