using System;
using System.Linq;
using Console.Core.CommandSystem;
using Console.Core.CommandSystem.Commands;

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
        [Command("add-command", "Adds all static Console Commands of the specified Type")]
        private static void AddCommands(string qualifiedName)
        {
            Type t = Type.GetType(qualifiedName);
            CommandAttributeUtils.AddCommands(t);
        }

        [Command("remove-command", "Removes a command with this name and counts.")]
        private static void RemoveCommand(string name, int parameterCount, int flagCount)
        {
            AbstractCommand cd = CommandManager.GetCommand(name, parameterCount, flagCount);
            CommandManager.RemoveCommand(cd);
        }

        [Command("remove-command", "Removes a command with this name and parameter count.")]
        private static void RemoveCommand(string name, int parameterCount)
        {
            RemoveCommand(name, parameterCount, 0);
        }

        [Command("remove-command", "Removes a command with this name and no parameters.")]
        private static void RemoveCommand(string name)
        {
            RemoveCommand(name, 0);
        }

        [Command("remove-commands", "Removes all commands with this name.")]
        private static void RemoveCommands(string name)
        {
            CommandManager.GetCommands(name).ToList().ForEach(CommandManager.RemoveCommand);
        }
    }
}