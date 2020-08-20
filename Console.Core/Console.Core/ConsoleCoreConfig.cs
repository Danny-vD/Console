using System;
using System.Reflection;
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
        /// The Version of the Core Library
        /// </summary>
        [Property("version.core")]
        public static Version CoreVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Should the Console print the entered command?
        /// </summary>
        [Property("core.output.writecommand")]
        public static bool WriteCommand = true;

        /// <summary>
        /// The Prefix of Commands.
        /// </summary>
        [Property("core.input.prefix")]
        public static string ConsolePrefix = "";

        /// <summary>
        /// The Character that is used to enclose string blocks.
        /// </summary>
        [Property("core.input.stringchar")]
        public static char StringChar = '"';

        /// <summary>
        /// The Character that is used to seperate input in the console.
        /// </summary>
        [Property("core.input.commandseparator")]
        public static char CommandInputSeparator = ' ';
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
        public static char[] EscapableChars => new[] { StringChar };

        /// <summary>
        /// If true the Console does not check if commands can be invoked or are hidden by other commands.
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
        public static int FindClosing(string cmd, char openBracket, char closeBracket, int start = 0, int openBrackets = 0)
        {
            int open = openBrackets;
            for (int i = start; i < cmd.Length; i++)
            {
                char c = cmd[i];
                if (c == openBracket) open++;
                else if (c == closeBracket)
                {
                    open--;
                    if (open == 0) return i;
                }
            }
            return -1;
        }


        #endregion

    }
}