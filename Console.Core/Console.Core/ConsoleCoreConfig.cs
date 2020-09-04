using System;
using System.Collections.Generic;
using System.Reflection;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;

namespace Console.Core
{
    /// <summary>
    /// Core Configuration File of the Console System.
    /// </summary>
    public static class ConsoleCoreConfig
    {
        #region Console Properties

        /// <summary>
        /// Logger used to Write Logs from the Core Library
        /// </summary>
        internal static readonly ALogger CoreLogger =
            TypedLogger.CreateTypedWithPrefix(Assembly.GetExecutingAssembly().GetName().Name);

        /// <summary>
        /// The Version of the Core Library
        /// </summary>
        [Property("version.core")]
        public static Version CoreVersion => Assembly.GetExecutingAssembly().GetName().Version;



        /// <summary>
        /// Enables Optimizations without the OptimizeILAttribute
        /// </summary>
        [Property("core.il.aggressive")]
        public static bool AggressiveILOptimizations = false;
        /// <summary>
        /// Enables IL Method Call Optimizations
        /// </summary>
        [Property("core.il.enable")]
        public static bool EnableILOptimizations = false;

        /// <summary>
        /// Should the Console print the entered command?
        /// </summary>
        [Property("core.output.writecommand")] public static bool WriteCommand = false;

        /// <summary>
        /// If True will only write the exception message instead of the whole exception
        /// </summary>
        [Property("core.log.exception.messageonly")]
        public static bool LogExceptionMessageOnly = true;

        /// <summary>
        /// The Prefix of Commands.
        /// </summary>
        [Property("core.input.prefix")] public static string ConsolePrefix = "";

        /// <summary>
        /// The Character that is used to enclose string blocks.
        /// </summary>
        [Property("core.input.stringchar")]
        public static char StringChar
        {
            get => _stringChar;
            set
            {
                ReplaceChar(_stringChar, value);
                _stringChar = value;
            }
        }
        /// <summary>
        /// Backing field for the StringChar Property
        /// </summary>
        private static char _stringChar = '"';

        /// <summary>
        /// The Character that is used to seperate input in the console.
        /// </summary>
        [Property("core.input.commandseparator")]
        public static char CommandInputSeparator = ' ';
        /// <summary>
        /// The Character that is used to seperate input in the console.
        /// </summary>
        [Property("core.input.flagprefix")]
        public static string CommandFlagPrefix = "--";

        /// <summary>
        /// The Character that is used to escape the EscapableChars
        /// </summary>
        [Property("core.input.escapechar")]
        public static char EscapeChar = '\\';

        /// <summary>
        /// Console Newline Character
        /// </summary>
        public static char NewLine = '\n';

        /// <summary>
        /// Collection of Characters that have to be Escaped.
        /// </summary>
        public static char[] EscapableChars => _escapableChars.ToArray();

        /// <summary>
        /// Backing Field of the Escapable Character Array
        /// </summary>
        private static readonly List<char> _escapableChars = new List<char> { StringChar };

        /// <summary>
        /// Adds a Character to the EscapeChars
        /// </summary>
        /// <param name="escChar">Character to add</param>
        public static void AddEscapeChar(char escChar)
        {
            if (!_escapableChars.Contains(escChar))
            {
                _escapableChars.Add(escChar);
            }
        }

        /// <summary>
        /// Removes a Character from the Escape Chars
        /// </summary>
        /// <param name="escChar"></param>
        public static void RemoveEscapeChar(char escChar)
        {
            if (_escapableChars.Contains(escChar))
            {
                _escapableChars.Remove(escChar);
            }
        }

        /// <summary>
        /// Replaces a Character from the Escape Chars
        /// </summary>
        /// <param name="oldChar">Character to Replace</param>
        /// <param name="newChar">Replacement Char</param>
        public static void ReplaceChar(char oldChar, char newChar)
        {
            RemoveEscapeChar(oldChar);
            AddEscapeChar(newChar);
        }

        /// <summary>
        /// If true the Console does not check if Commands can be invoked or are hidden by other Commands.
        /// </summary>
        [Property("core.commands.allowoverlapping")]
        public static bool AllowOverlappingCommands;


        /// <summary>
        /// Finds the Corresponding Closing Tag
        /// </summary>
        /// <param name="cmd">Input Command.</param>
        /// <param name="openBracket">The Character used to Open a Block</param>
        /// <param name="closeBracket">The Character used to Close a Block</param>
        /// <param name="start">The Start index from where the search begins</param>
        /// <param name="openBrackets">The Amount of open brackets that were already found.</param>
        /// <returns>Index of the Corresponding Closing Tag</returns>
        public static int FindClosing(string cmd, char openBracket, char closeBracket, int start = 0,
            int openBrackets = 0)
        {
            int open = openBrackets;
            for (int i = start; i < cmd.Length; i++)
            {
                char c = cmd[i];
                if (c == openBracket)
                {
                    open++;
                }
                else if (c == closeBracket)
                {
                    open--;
                    if (open == 0)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        #endregion
    }
}