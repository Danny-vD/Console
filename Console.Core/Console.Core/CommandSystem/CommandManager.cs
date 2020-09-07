using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Console.Core.CommandSystem.Commands;
using Console.Core.PropertySystem;

namespace Console.Core.CommandSystem
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum AmbiguityResolveResponse
    {

        /// <summary>
        /// 
        /// </summary>
        Resolve = 0,

        /// <summary>
        /// 
        /// </summary>
        Throw = 1,

        /// <summary>
        /// 
        /// </summary>
        Error = 2,

        /// <summary>
        /// 
        /// </summary>
        Warning = 4

    }

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum AmbiguityResolveStrategy
    {

        /// <summary>
        /// 
        /// </summary>
        Alphabetically = 8,

        /// <summary>
        /// 
        /// </summary>
        Priority = 16

    }

    /// <summary>
    /// Contains all Loaded Commands and Implements the Core Command Logic like Adding/Removing/Renaming/...
    /// </summary>
    public static class CommandManager
    {

        /// <summary>
        /// 
        /// </summary>
        [Property("core.commands.ambiguity.response")]
        public static AmbiguityResolveResponse AmbiguityResponse = AmbiguityResolveResponse.Error;

        /// <summary>
        /// 
        /// </summary>
        [Property("core.commands.ambiguity.strategy")]
        public static AmbiguityResolveStrategy AmbiguityStrategy = AmbiguityResolveStrategy.Alphabetically;

        /// <summary>
        /// All Commands in the Console System.
        /// </summary>
        public static List<AbstractCommand> AllCommands = new List<AbstractCommand>();

        private static int ambiguityFlags => (int) AmbiguityResponse | (int) AmbiguityStrategy;


        //public static List<AbstractCommand> GetFilteredCommands(string filterQuery) => CommandFilter.Filter(filterQuery, AllCommands);


        /// <summary>
        /// Invokes a given command with given parameters (does not respect user-defined conversions between types, except IConvertible)
        /// </summary>
        /// <param name="commandName">Name of the Command to Invoke</param>
        /// <param name="parameters">The Command Parameters</param>
        public static void Invoke(string commandName, params string[] parameters)
        {
            // 16 is the max amount of parameters we allow, because system.Action only goes up to 16 generics
            int paramsCount = parameters.Count(x => !x.StartsWith(ConsoleCoreConfig.CommandFlagPrefix));
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

            //if (FilteredCommands.Any(item => item.Identity.ContainsName(newName)))
            //{
            //    ConsoleCoreConfig.CoreLogger.LogError($"Rename failed! A command with name {newName} already exists!");
            //    return;
            //}

            command.Identity.Replace(commandName, newName);
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

            //if (FilteredCommands.Any(item => item.Identity.ContainsName(alias)))
            //{
            //    ConsoleCoreConfig.CoreLogger.LogError($"Rename failed! A command with name {alias} already exists!");
            //    return;
            //}


            command.Identity.AddName(alias);
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

            command.Identity.RemoveName(alias);
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
            List<AbstractCommand> result = new List<AbstractCommand>();


            foreach (AbstractCommand abstractCommand in AllCommands)
            {
                if (!abstractCommand.Identity.FilteredContainsName(commandName))
                {
                    continue;
                }

                int pc = paramsCount - flagCount;
                if (abstractCommand is ReflectionCommand refl)
                {
                    if (abstractCommand.ParametersCount.Min == pc)
                    {
                        if (refl.ParametersCount.Contains(paramsCount))
                        {
                            result.Add(refl);
                        }
                    }
                }
                else if (abstractCommand.ParametersCount.Contains(paramsCount))
                {
                    result.Add(abstractCommand);
                }
            }

            if (result.Count > 1)
            {
                if (NamespaceDiffers(result))
                {
                    if ((ambiguityFlags & (int) AmbiguityResolveResponse.Warning) != 0)
                    {
                        ConsoleCoreConfig.CoreLogger.LogWarning(ToString(result));
                    }

                    if ((ambiguityFlags & (int) AmbiguityResolveResponse.Error) != 0)
                    {
                        ConsoleCoreConfig.CoreLogger.LogError(ToString(result));
                    }

                    if ((ambiguityFlags & (int) AmbiguityResolveResponse.Throw) != 0)
                    {
                        throw new Exception(ToString(result));
                    }
                }

                if ((ambiguityFlags & (int) AmbiguityResolveStrategy.Alphabetically) != 0)
                {
                    return result.OrderBy(x => x.Identity.QualifiedName).First();
                }

                return CommandFilter.Prioritize(result).FirstOrDefault();
            }

            return result.FirstOrDefault();

            string ToString(List<AbstractCommand> commands)
            {
                StringBuilder sb = new StringBuilder("Conflicts:\n");
                foreach (AbstractCommand abstractCommand in commands)
                {
                    sb.AppendLine("\t" + Unpack(abstractCommand.Identity.QualifiedNames.ToList()));
                }

                return sb.ToString();
            }

            bool NamespaceDiffers(List<AbstractCommand> commands)
            {
                string name = commands.First().Identity.Namespace;
                for (int i = 1; i < commands.Count; i++)
                {
                    if (commands[i].Identity.Namespace != name)
                    {
                        return true;
                    }
                }

                return false;
            }

            string Unpack(List<string> list)
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
                                                        $"A command with name {commandName} and {paramsCount} parameter(s) does not exist!\n"
                                                       );
            }

            return command;
        }


        /// <summary>
        /// Returns a list of all Commands with a given name
        /// </summary>
        /// <param name="commandName">Command Name</param>
        /// <param name="find">Should the Method also contain Commands that contain the command name?</param>
        /// <returns>Commands that fit the search criteria</returns>
        public static IEnumerable<AbstractCommand> GetCommands(string commandName, bool find = false)
        {
            List<AbstractCommand> tempCommands = AllCommands
                                                 .Where(
                                                        command =>
                                                            command.Identity.FilteredContainsName(commandName, find)
                                                       )
                                                 .ToList();

            if (tempCommands.Count == 0)
            {
                string s = $"A command with name {commandName} does not exist!\n";
                ConsoleCoreConfig.CoreLogger.LogWarning(s);
            }

            return tempCommands;
        }

        private static void SortCommands()
        {
            AllCommands = AllCommands.OrderBy(x => x.Identity.QualifiedName).ToList();
        }


        private static bool CompletelyHiding(AbstractCommand cmd, AbstractCommand other)
        {
            return cmd.Identity.QualifiedName == other.Identity.QualifiedName;
        }

        /// <summary>
        /// Adds a Command to the Console System.
        /// </summary>
        /// <param name="command">Command to Add</param>
        public static void AddCommand(AbstractCommand command)
        {
            if (command.Identity.QualifiedNames.Any(name => name.Contains(" ")))
            {
                ConsoleCoreConfig.CoreLogger.LogError("A command name cannot contain a whitespace character!");
                return;
            }


            AbstractCommand
                cmd = null; //Commands.FirstOrDefault(item => item.HasName(command.GetAllNames()) && item.ParametersCount.Contains(command.ParametersCount.Max-command.FlagAttributeCount));

            for (int i = 0; i < AllCommands.Count; i++)
            {
                AbstractCommand item = AllCommands[i];
                if (CompletelyHiding(item, command) && item.ParametersCount.Min == command.ParametersCount.Min)
                {
                    cmd = item;
                    break;
                }
            }

            if (cmd != null)
            {
                if (ConsoleCoreConfig.DisableOverlappingCommandsLogs) return;
                if (ConsoleCoreConfig.AllowOverlappingCommands)
                {
                    ConsoleCoreConfig.CoreLogger.LogWarning(
                        $"A command with name {command.Identity.QualifiedName} with {cmd.ParametersCount} parameter(s) already exists!");
                }
                else
                {
                    ConsoleCoreConfig.CoreLogger.LogError(
                        $"A command with name {command.Identity.QualifiedName} with {cmd.ParametersCount} parameter(s) already exists!");
                }
                return;
            }

            AllCommands.Add(command);
            SortCommands();
        }

        /// <summary>
        /// Removes a Command from the Console System.
        /// </summary>
        /// <param name="commandName">Command Name</param>
        /// <param name="paramsCount">Parameter Count</param>
        public static void RemoveCommand(string commandName, int paramsCount)
        {
            AbstractCommand command = GetCommand(commandName, paramsCount);

            AllCommands.Remove(command);
        }


        /// <summary>
        /// Removes the Specified Command from the Console System.
        /// </summary>
        /// <param name="command">Command to Remove</param>
        public static void RemoveCommand(AbstractCommand command)
        {
            AllCommands.Remove(command);
        }

    }
}