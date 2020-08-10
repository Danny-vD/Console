using System;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Utils;

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