using System;

namespace Console.Core.Attributes.CommandSystem
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class CommandFlagAttribute : Attribute
    {
        public readonly string Name;

        public CommandFlagAttribute(string name = null)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name != null ? $"CommandFlag({Name})" : "CommandFlag";
        }
    }
}