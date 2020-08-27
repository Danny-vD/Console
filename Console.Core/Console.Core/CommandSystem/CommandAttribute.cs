using System;

namespace Console.Core.CommandSystem
{
    /// <summary>
    /// Command Attribute that only works on methods.
    /// Allows multiple Attributes on the same methods(this is a fancy way to create aliases without using the alias system :D)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Name of the Command.
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Help Message of the Command
        /// </summary>
        public readonly string HelpMessage;
        /// <summary>
        /// Command Aliases
        /// </summary>
        public readonly string[] Aliases;

        /// <summary>
        /// Creates a Command with the Specified Settings.
        /// </summary>
        /// <param name="name">Name of the Command, Null if default name(same as the Method Name)</param>
        /// <param name="helpMessage">Help Message that gets displayed in the Help Command</param>
        /// <param name="alias">Optional Additional Command Names and Shortcuts</param>
        public CommandAttribute(string name = null, string helpMessage = "No Help Message", params string[] alias)
        {
            Name = name;
            HelpMessage = helpMessage;
            Aliases = alias;
        }
    }
}