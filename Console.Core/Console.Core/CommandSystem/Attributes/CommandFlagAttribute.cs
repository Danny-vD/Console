using System;

namespace Console.Core.CommandSystem.Attributes
{
    /// <summary>
    /// Can be used to optionally allow specifying of parameters in the Switch Syntax
    /// Parameters of type Boolean can be decorated with this, can be specified by specifying the parameter name
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class CommandFlagAttribute : ConsoleAttribute
    {

        /// <summary>
        /// Name of the Flag
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Creates a CommandFlag with the specified name or the parameter name(when no name specified)
        /// </summary>
        /// <param name="name">Name of the Flag(Empty for Parameter Name)</param>
        public CommandFlagAttribute(string name = null)
        {
            Name = name;
        }

        /// <summary>
        /// To String Implementation
        /// </summary>
        /// <returns>String Representation</returns>
        public override string ToString()
        {
            return Name != null ? $"CommandFlag({Name})" : "CommandFlag";
        }

    }
}