using System;
using Console.Core.CommandSystem;

namespace Console.UtilExtension
{
    /// <summary>
    /// Utility Commands for the Command Systems
    /// </summary>
    public class UtilCommandCommands
    {
        /// <summary>
        /// Adds all commands of a specified type
        /// </summary>
        /// <param name="qualifiedName">Assembly Qualified Type Name</param>
        [Command("add-commands", "Adds all static Console Commands of the specified Type")]
        private static void AddCommands(string qualifiedName)
        {
            Type t = Type.GetType(qualifiedName);
            CommandAttributeUtils.AddCommands(t);
        }
    }
}