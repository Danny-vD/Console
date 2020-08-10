using System;
using System.Reflection;
using Console.Core.PropertySystem;

namespace Console.Core
{
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

        [Property("core.output.writecommand")]
        public static bool WriteCommand = true;

        [Property("core.input.prefix")]
        public static string ConsolePrefix = "";

        [Property("core.input.stringchar")]
        public static char StringChar = '"';

        #endregion

    }
}