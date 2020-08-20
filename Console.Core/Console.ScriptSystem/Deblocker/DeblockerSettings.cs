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
        /// Open Block Bracket
        /// </summary>
        [Property("console.scripts.block.open")]
        public static char BlockBracketOpen = '{';
        /// <summary>
        /// Close Block Bracket
        /// </summary>
        [Property("console.scripts.block.close")]
        public static char BlockBracketClosed = '}';
        /// <summary>
        /// Helper Function that provides a Unique Key Sequence
        /// </summary>
        /// <param name="index">Block Index</param>
        /// <returns>String Representation if the Block Index for Replacement later.</returns>
        public static string GetKey(int index) => "###BLOCK" + index + "###";

        /// <summary>
        /// Private Counter to create Unique Block Names
        /// </summary>
        private static int BlockCount = 0;

        /// <summary>
        /// Returns a Unique Block Name
        /// </summary>
        /// <returns>Unique Block Name: e.g.: BLOCK_0 </returns>
        public static string GetBlockName() => $"BLOCK_{BlockCount++}";
    }
}