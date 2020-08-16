using System;
using System.Reflection;
using Console.Core.PropertySystem;

namespace Console.Core
{
    /// <summary>
    /// Core Configuration File of the Console System.
    /// </summary>
    public class ConsoleCoreConfig
    {
        private ConsoleCoreConfig()
        {
        }


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

        #endregion

    }
}