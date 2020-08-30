using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Core.CommandSystem.Commands;

namespace Console.Core.CommandSystem.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandBuilder
    {

        internal static List<AutoFillProvider> LoadedAutoFillers = new List<AutoFillProvider>();
        private List<AutoFillProvider> autoFillers
        {
            get
            {
                AbstractCommand[] cmds = Commands;
                List<AutoFillProvider> prov = new List<AutoFillProvider>();
                foreach (AbstractCommand abstractCommand in cmds)
                {
                    AutoFillProvider[] pros = LoadedAutoFillers.Where(x => x.CanFill(this, abstractCommand, CommandPartNum))
                        .ToArray();
                    foreach (AutoFillProvider autoFillProvider in pros)
                    {
                        if (!prov.Contains(autoFillProvider))
                            prov.Add(autoFillProvider);
                    }
                }
                return prov;
            }
        }

        private AbstractCommand[] Commands => CommandManager.commands.Where(x => x.HasName(GetPart(0), true)).ToArray();

        private (int, int) GetPartBounds(int part)
        {
            string cmd = builder.ToString();
            bool inQuotes = false;
            int last = 0;
            int partCount = 0;
            //List<string> parts = new List<string>();
            for (int i = 0; i < cmd.Length; i++)
            {
                if (cmd[i] == ' ' && !inQuotes)
                {
                    if (part == partCount)
                    {
                        if (last == 0)
                            return (last, i - last);
                        else
                            return (last + 1, i - last - 1);
                    }
                    partCount++;
                    last = i;
                    //parts.Add(cmd.Substring(last, i - last));
                }
                if (cmd[i] == ConsoleCoreConfig.StringChar && !CommandParser.IsEscaped(cmd, i))
                {
                    if (inQuotes)
                    {
                        inQuotes = false;
                        if (part == partCount)
                        {
                            if (last == 0)
                                return (last, i - last);
                            else
                                return (last + 1, last - i - 1);
                        }
                        partCount++;
                        //parts.Add(cmd.Substring(last, i - last));
                    }
                    else
                    {
                        inQuotes = true;
                        last = i;
                    }
                }
            }

            if (last == 0)
                return (last, cmd.Length - last);
            else
                return (last + 1, cmd.Length - last - 1);
            //parts.Add(cmd.Substring(last, cmd.Length - last));

            //if (part >= 0 && part < parts.Count)
            //    return parts[part];
            //return cmd;
        }

        private string GetPart(int part)
        {
            (int start, int length) pb = GetPartBounds(part);
            string s = builder.ToString().Substring(pb.start, pb.length);
            return s;
        }

        private string[] GetPossibleAutoFills(AbstractCommand cmd, string start)
        {
            List<string> ret = new List<string>();
            foreach (AutoFillProvider autoFillProvider in autoFillers)
            {
                string[] f = autoFillProvider.GetAutoFills(cmd, CommandPartNum, start);
                foreach (string s in f)
                {
                    if (!ret.Contains(s)) ret.Add(s);
                }
            }
            return ret.ToArray();
        }

        private readonly StringBuilder builder = new StringBuilder();

        private int CommandPartNum { get; set; }
        private string CurrentPart => GetPart(CommandPartNum);
        private int StringChars { get; set; }
        private bool NextIsEscaped = false;

        private string[] CompleteList;
        private int CompleteNum;
        private bool ValidComplete;

        private string GetComplete()
        {
            if (ValidComplete)
            {
                if (CompleteList.Length == 0)
                    return "";
                return CompleteList[CompleteNum % CompleteList.Length];
            }
            List<string> fills = new List<string>();
            foreach (AbstractCommand abstractCommand in Commands)
            {
                string[] s = GetPossibleAutoFills(abstractCommand, CurrentPart);
                foreach (string s1 in s)
                {
                    if (!fills.Contains(s1)) fills.Add(s1);
                }
            }
            CompleteList = fills.ToArray();
            ValidComplete = true;
            CompleteNum = 0;
            return GetComplete();
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Complete { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public void Remove()
        {
            if (builder.Length != 0)
            {
                ValidComplete = false;
                builder.Remove(builder.Length - 1, 1);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void CompletePart()
        {
            //Auto Fill
            string s = GetComplete();
            CompleteNum++;
            if (!string.IsNullOrEmpty(s))
            {
                (int start, int length) pb = GetPartBounds(CommandPartNum);
                builder.Remove(pb.start, pb.length);
                builder.Append(s);
                //foreach (char c in s)
                //{
                //    Append(c);
                //}
            }
        }

        private int FindLastSpace(StringBuilder sb, int startIndex)
        {
            for (int i = startIndex; i >= 0; i--)
            {
                if (sb[i] == ' ') return i;
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void Append(char character)
        {
            ValidComplete = false;
            if (!NextIsEscaped)
            {
                if (character == ConsoleCoreConfig.StringChar)
                {
                    StringChars++;
                    if (StringChars % 2 == 0) CommandPartNum++;
                }
            }

            if (character == ' ')
            {
                if (StringChars % 2 == 0)
                {
                    CommandPartNum++;
                }
            }

            if (character == ConsoleCoreConfig.EscapeChar)
            {
                NextIsEscaped = true;
            }
            else
            {
                NextIsEscaped = false;
            }

            if (character == ConsoleCoreConfig.NewLine)
            {
                Complete = true;
                return;
            }
            builder.Append(character);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CreateCommand()
        {
            CommandBuilder cb = new CommandBuilder();
            int pos = System.Console.CursorLeft;
            while (!cb.Complete)
            {
                ConsoleKeyInfo ki = System.Console.ReadKey();
                if (ki.Key == ConsoleKey.Escape)
                {
                    cb = new CommandBuilder();
                    CleanLine(pos);
                    System.Console.CursorLeft = pos;
                    continue;
                }
                if (ki.Key == ConsoleKey.Enter)
                    break;
                if (ki.Key == ConsoleKey.Backspace)
                {
                    if (System.Console.CursorLeft < pos)
                    {
                        System.Console.CursorLeft = pos;
                        continue;
                    }
                    System.Console.Write(' ');
                    System.Console.CursorLeft--;
                    cb.Remove();
                    continue;
                }
                if (ki.Key == ConsoleKey.Tab)
                {
                    cb.CompletePart();
                }
                else
                {
                    cb.Append(ki.KeyChar);
                }
                CleanLine(pos);
                System.Console.CursorLeft = pos;
                System.Console.Write(cb.ToString());

            }
            return cb.ToString();
        }

        private static void CleanLine(int start, int length = -1)
        {
            System.Console.CursorLeft = start;
            int len = length == -1 ? System.Console.WindowWidth - start - 1 : length;
            for (int i = 0; i < len; i++)
            {
                System.Console.Write(' ');
            }
        }
    }
}