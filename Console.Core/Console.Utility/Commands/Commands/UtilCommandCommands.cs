using System;
using System.Linq;

using Console.Core.CommandSystem;
using Console.Core.CommandSystem.Attributes;
using Console.Core.CommandSystem.Commands;

namespace Console.Utility.Commands.Commands
{
    /// <summary>
    /// Utility Commands for the Command Systems
    /// </summary>
    public class UtilCommandCommands
    {
        private const string COMMAND_NAMESPACE = UtilExtensionInitializer.UTIL_NAMESPACE + "::commands";

        /// <summary>
        /// Adds all commands of a specified type
        /// </summary>
        /// <param name="qualifiedName">Assembly Qualified Type Name</param>
        [Command(
            "add",
            Namespace = COMMAND_NAMESPACE,
            HelpMessage = "Adds all static Console Commands of the specified Type"
        )]
        private static void AddCommands(string qualifiedName)
        {
            Type t = Type.GetType(qualifiedName);
            CommandAttributeUtils.AddCommands(t);
        }

        [Command(
            "remove",
            Namespace = COMMAND_NAMESPACE,
            HelpMessage = "Removes a command with this name and counts."
        )]
        private static void RemoveCommand(string name, int parameterCount, int flagCount)
        {
            AbstractCommand cd = CommandManager.GetCommand(name, parameterCount, flagCount);
            CommandManager.RemoveCommand(cd);
        }

        [Command(
            "remove",
            Namespace = COMMAND_NAMESPACE,
            HelpMessage = "Removes a command with this name and parameter count."
        )]
        private static void RemoveCommand(string name, int parameterCount)
        {
            RemoveCommand(name, parameterCount, 0);
        }

        [Command(
            "remove",
            Namespace = COMMAND_NAMESPACE,
            HelpMessage = "Removes a command with this name and no parameters."
        )]
        private static void RemoveCommand(string name)
        {
            RemoveCommand(name, 0);
        }

        [Command(
            "remove-commands",
            Namespace = COMMAND_NAMESPACE,
            HelpMessage = "Removes all commands with this name."
        )]
        private static void RemoveCommands(string name)
        {
            CommandManager.GetCommands(name).ToList().ForEach(CommandManager.RemoveCommand);
        }

    }
}