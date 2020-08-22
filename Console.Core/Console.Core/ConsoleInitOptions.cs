using System;

namespace Console.Core
{
    /// <summary>
    /// Console Startup Settings
    /// Defines which default commands are added.
    /// </summary>
    [Flags]
    public enum ConsoleInitOptions
    {
        /// <summary>
        /// Add all Default Commands
        /// </summary>
        All = -1 & ~FlagTests,
        /// <summary>
        /// Do not add any Default Commands
        /// </summary>
        None = 0,
        /// <summary>
        /// Clear / Help and Echo Command
        /// </summary>
        DefaultCommands = 1,
        /// <summary>
        /// Commands that allow interfacing with the Property System
        /// </summary>
        PropertyCommands = 2,
        /// <summary>
        /// Commands that allow Loading Extensions from the Commandline
        /// </summary>
        ExtensionCommands = 4,
        /// <summary>
        /// Commands that allow viewing and clearing the selected object list
        /// </summary>
        SelectionCommands = 8,
        /// <summary>
        /// Commands that Test the behaviour of Flag Attributes.
        /// </summary>
        FlagTests = 16,
    }
}