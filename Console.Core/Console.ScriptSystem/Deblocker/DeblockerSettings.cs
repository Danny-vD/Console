using System.Reflection;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;

/// <summary>
/// Namespace containing the Logic for the Script System Parsing of Brackets
/// </summary>
namespace Console.ScriptSystem.Deblocker
{
    /// <summary>
    /// The Settings of the DeblockerCollection
    /// </summary>
    public static class DeblockerSettings
    {
        /// <summary>
        /// Mutes all Deblockerk System Logs
        /// </summary>
        [Property("logs.scriptsystem.deblocker.mute")]
        private static bool MuteLogs
        {
            get => Logger.Mute;
            set => Logger.Mute = value;
        }

        [Property("logs.scriptsystem.deblocker.verbose")]
        private static bool VerboseDeblockOutput = false;

        internal static void LogVerbose(string msg)
        {
            if (VerboseDeblockOutput)
            {
                Logger.Log(msg);
            }
        }

        /// <summary>
        /// Logger used by the Deblocker System
        /// </summary>
        public static ALogger Logger => _logger ?? (_logger = CreateLogger());

        private static ALogger CreateLogger()
        {
            ALogger l= TypedLogger.CreateTypedWithPrefix("Deblocker");
            l.Mute = true;
            return l;
        }

        private static ALogger _logger;

        /// <summary>
        /// Open Block Bracket
        /// </summary>
        [Property("scriptsystem.block.open")]
        public static char BlockBracketOpen = '{';
        /// <summary>
        /// Close Block Bracket
        /// </summary>
        [Property("scriptsystem.block.close")]
        public static char BlockBracketClosed = '}';


        /// <summary>
        /// The Open Tag for the Function Signature
        /// </summary>
        [Property("scriptsystem.function.open")]
        public static char OpenFunctionBracket = '(';


        /// <summary>
        /// The Close Tag for the Function Signature
        /// </summary>
        [Property("scriptsystem.function.close")]
        public static char CloseFunctionBracket = ')';

        /// <summary>
        /// Helper Function that provides a Unique Key Sequence
        /// </summary>
        /// <param name="index">Block Index</param>
        /// <returns>String Representation if the Block Index for Replacement later.</returns>
        public static string GetKey(int index) => $"###BLOCK{index}###";

        /// <summary>
        /// Private Counter to create Unique Block Names
        /// </summary>
        private static int BlockCount = 0;

        /// <summary>
        /// Returns a Unique Block Name
        /// </summary>
        /// <returns>Unique Block Name: e.g.: BLOCK_0 </returns>
        public static string GetBlockName() => $"{BLOCK_NAME_BEGIN}{BlockCount++}";

        public const string BLOCK_NAME_BEGIN = "BLOCK_";
    }
}