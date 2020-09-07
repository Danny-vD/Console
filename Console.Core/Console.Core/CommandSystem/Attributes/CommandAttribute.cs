using System;

namespace Console.Core.CommandSystem.Attributes
{
    /// <summary>
    /// Command Attribute that only works on methods.
    /// Allows multiple Attributes on the same methods(this is a fancy way to create aliases without using the alias system :D)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAttribute : ConsoleAttribute
    {

        /// <summary>
        /// Command Aliases
        /// </summary>
        public string[] Aliases = new string[0];

        /// <summary>
        /// Help Message of the Command
        /// </summary>
        public string HelpMessage = "No Help Message";

        /// <summary>
        /// The Primary Name of the Command.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// The Namespace of the Command
        /// </summary>
        public string Namespace = "";

        /// <summary>
        /// Creates a Command with the Specified Settings.
        /// </summary>
        /// <param name="name">Name of the Command, Null if default name(same as the Method Name)</param>
        public CommandAttribute(string name)
        {
            Name = name;
        }

    }
}