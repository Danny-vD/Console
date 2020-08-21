using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Console.Core.CommandSystem.Commands
{

    /// <summary>
    /// Contains all Loaded Commands and Implements the Core Command Logic like Adding/Removing/Renaming/...
    /// </summary>
    public static class CommandManager
    {
        /// <summary>
        /// All Commands in the Console System.
        /// </summary>
        public static readonly List<AbstractCommand> commands = new List<AbstractCommand>();

        /// <summary>
        /// Invokes a given command with given parameters (does not respect user-defined conversions between types, except IConvertible)
        /// </summary>
        /// <param name="commandName">Name of the Command to Invoke</param>
        /// <param name="parameters">The Command Parameters</param>
        public static void Invoke(string commandName, params string[] parameters)
        {
            // 16 is the max amount of parameters we allow, because system.Action only goes up to 16 generics
            int paramsCount = parameters.Count(x => !x.StartsWith("-"));
            AbstractCommand command = GetCommand(commandName, parameters.Length, parameters.Length - paramsCount);

            command?.Invoke(parameters);
        }

        /// <summary>
        /// Renames the Specified Command.
        /// </summary>
        /// <param name="commandName">Old Command Name</param>
        /// <param name="paramsCount">Parameter Count</param>
        /// <param name="newName">New Command Name</param>
        public static void RenameCommand(string commandName, int paramsCount, string newName)
        {
            AbstractCommand command = GetCommand(commandName, paramsCount);

            if (commands.Any(item => item.HasName(newName)))
            {
                ConsoleCoreConfig.CoreLogger.LogError($"Rename failed! A command with name {newName} already exists!");
                return;
            }

            command.SetName(newName);
        }

        /// <summary>
        /// Adds an Alias to the specified command.
        /// </summary>
        /// <param name="commandName">Command Name</param>
        /// <param name="paramsCount">Parameter Count</param>
        /// <param name="alias">New Alias</param>
        public static void AddAlias(string commandName, int paramsCount, string alias)
        {
            AbstractCommand command = GetCommand(commandName, paramsCount);

            if (commands.Any(item => item.HasName(alias)))
            {
                ConsoleCoreConfig.CoreLogger.LogError($"Rename failed! A command with name {alias} already exists!");
                return;
            }

            command.AddAlias(alias);
        }

        /// <summary>
        /// Removes an Alias from the specified command.
        /// </summary>
        /// <param name="commandName">Command Name</param>
        /// <param name="paramsCount">Parameter Count</param>
        /// <param name="alias">Alias to Remove</param>
        public static void RemoveAlias(string commandName, int paramsCount, string alias)
        {
            AbstractCommand command = GetCommand(commandName, paramsCount);

            command.RemoveAlias(alias);
        }

        /// <summary>
        /// Finds a Command that has the specified name and parameter count
        /// </summary>
        /// <param name="commandName">Command Name</param>
        /// <param name="paramsCount">Parameter Count</param>
        /// <param name="flagCount">The amount of Parameters with the CommandFlagAttribute</param>
        /// <returns>Returns the Command that Fits the Name/Parameter Count Combination</returns>
        private static AbstractCommand Find(string commandName, int paramsCount, int flagCount = 0)
        {
            foreach (AbstractCommand abstractCommand in commands)
            {
                if (!abstractCommand.HasName(commandName)) continue;

                int pc = paramsCount - flagCount;
                if (abstractCommand is ReflectionCommand refl)
                {
                    if (abstractCommand.ParametersCount.Min == pc)
                    {
                        int flagC = refl.FlagAttributeCount - (paramsCount - refl.ParametersCount.Min);
                        int parC = paramsCount + refl.SelectionAttributeCount + flagC;
                        if (refl.ParametersCount.Contains(paramsCount))
                        {
                            return refl;
                        }
                    }
                }
                else if (abstractCommand.ParametersCount.Contains(paramsCount))
                {
                    return abstractCommand;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the respective command with a given name and specified amount of parameters
        /// </summary>
        /// <param name="commandName">Command Name</param>
        /// <param name="paramsCount">Parameter Count</param>
        /// <param name="flagCount">The amount of Parameters with the CommandFlagAttribute</param>
        /// <returns>Command that fits the Search Criteria</returns>
        public static AbstractCommand GetCommand(string commandName, int paramsCount, int flagCount = 0)
        {
            AbstractCommand command = Find(commandName, paramsCount, flagCount);

            if (command == null)
            {
                ConsoleCoreConfig.CoreLogger.LogWarning(
                    $"A command with name {commandName} and {paramsCount} parameter(s) does not exist!\n");
            }

            return command;
        }


        /// <summary>
        /// Returns a list of all commands with a given name
        /// </summary>
        /// <param name="commandName">Command Name</param>
        /// <param name="find">Should the Method also contain commands that contain the command name?</param>
        /// <returns>Commands that fit the search criteria</returns>
        public static IEnumerable<AbstractCommand> GetCommands(string commandName, bool find = false)
        {

            List<AbstractCommand> tempCommands;
            if (find)
            {
                tempCommands = commands.Where(command => command.Name.Contains(commandName)).ToList();
            }
            else
            {
                tempCommands = commands.Where(command => command.HasName(commandName)).ToList();
            }

            if (tempCommands.Count == 0)
            {
                string s = $"A command with name {commandName} does not exist!\n";
                ConsoleCoreConfig.CoreLogger.LogWarning(s);
            }

            return tempCommands;
        }

        /// <summary>
        /// Adds a Command to the Console System.
        /// </summary>
        /// <param name="command">Command to Add</param>
        public static void AddCommand(AbstractCommand command)
        {
            if (command.GetAllNames().Any(name => name.Contains(" ")))
            {
                ConsoleCoreConfig.CoreLogger.LogError("A command name cannot contain a whitespace character!");
                return;
            }


            AbstractCommand cmd = null;//commands.FirstOrDefault(item => item.HasName(command.GetAllNames()) && item.ParametersCount.Contains(command.ParametersCount.Max-command.FlagAttributeCount));

            for (int i = 0; i < commands.Count; i++)
            {
                AbstractCommand item = commands[i];
                if (item.HasName(command.GetAllNames()) && item.ParametersCount.Min == command.ParametersCount.Min)
                {
                    cmd = item;
                    break;
                }
            }

            if (cmd != null)
            {
                if(ConsoleCoreConfig.AllowOverlappingCommands)
                    ConsoleCoreConfig.CoreLogger.LogWarning(
                        $"A command with name {ToString(command.GetAllNames())} with {cmd.ParametersCount} parameter(s) already exists!");
                else
                    ConsoleCoreConfig.CoreLogger.LogError(
                        $"A command with name {ToString(command.GetAllNames())} with {cmd.ParametersCount} parameter(s) already exists!");
                return;
            }

            commands.Add(command);

            string ToString(List<string> list)
            {
                if (list.Count == 0)
                {
                    return string.Empty;
                }

                StringBuilder stringBuilder = new StringBuilder(list[0]);

                for (int i = 1; i < list.Count; i++)
                {
                    stringBuilder.Append($", {list[i]}");
                }

                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Removes a Command from the Console System.
        /// </summary>
        /// <param name="commandName">Command Name</param>
        /// <param name="paramsCount">Parameter Count</param>
        public static void RemoveCommand(string commandName, int paramsCount)
        {
            AbstractCommand command = GetCommand(commandName, paramsCount);

            commands.Remove(command);
        }


        /// <summary>
        /// Removes the Specified Command from the Console System.
        /// </summary>
        /// <param name="command">Command to Remove</param>
        public static void RemoveCommand(AbstractCommand command)
        {
            commands.Remove(command);
        }
    }
}