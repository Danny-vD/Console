using System;
using Console.Core.CommandSystem;

namespace Console.UtilExtension
{
    public class UtilCommandCommands
    {
        [Command("add-commands", "Adds all static Console Commands of the specified Type")]
        private static void AddCommands(string qualifiedName)
        {
            Type t = Type.GetType(qualifiedName);
            CommandAttributeUtils.AddCommands(t);
        }
    }
}