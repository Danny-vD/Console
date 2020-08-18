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
        [Property("core.input.escapechar")]
        public static char EscapeChar = '\\';
        public static char[] EscapableChars => new[] { StringChar, EscapeChar };

        /// <summary>
        /// If true the Console does not check if commands can be invoked or are hidden by other commands.
        /// </summary>
        [Property("core.commands.allowoverlapping")]
        public static bool AllowOverlappingCommands;

        #endregion

    }
}