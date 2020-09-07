using System;

namespace Console.Core
{
    /// <summary>
    /// Console Startup Settings
    /// Defines which default Commands are added.
    /// </summary>
    [Flags]
    public enum ConsoleInitOptions
    {

        /// <summary>
        /// 
        /// </summary>
        Default = Core | Properties,

        /// <summary>
        /// 
        /// </summary>
        None = 0,

        /// <summary>
        /// 
        /// </summary>
        Loader = 1,

        /// <summary>
        /// 
        /// </summary>
        Core = 2,

        /// <summary>
        /// 
        /// </summary>
        Properties = 4,

        /// <summary>
        /// 
        /// </summary>
        Selection = 8,

    }
}