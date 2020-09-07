using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.CommandSystem.Attributes;
using Console.Core.CommandSystem.Commands;
using Console.Core.ILOptimizations;
using Console.Core.LogSystem;
using Console.Script.Async;
using Console.Script.Deblocker;
using Console.Script.Deblocker.Parameters;

namespace Console.Script
{
    /// <summary>
    /// Static SequenceSystem API
    /// </summary>
    public static class SequenceSystem
    {
        private const string SEQUENCE_SYSTEM_NAMESPACE = ScriptSystemInitializer.SCRIPT_SYSTEM_NAMESPACE + "::sequence";

        /// <summary>
        /// The Logger used by the Sequence System
        /// </summary>
        private static readonly ALogger SequenceOut = TypedLogger.CreateTypedWithPrefix(SEQUENCE_SYSTEM_NAMESPACE);

        private static readonly Dictionary<string, AsyncRunner> ActiveRoutines = new Dictionary<string, AsyncRunner>();


        /// <summary>
        /// Internal Dictionary of Sequences.
        /// </summary>
        private static readonly Dictionary<string, Sequence> Sequences = new Dictionary<string, Sequence>();


        static SequenceSystem()
        {
            AConsoleManager.OnConsoleTick += InnerAsyncRun;
        }


        /// <summary>
        /// All Sequences listed like they get displayed on the console.
        /// </summary>
        public static string SequenceText
        {
            get
            {
                StringBuilder sb = new StringBuilder("Sequences:\n");
                foreach (string sequencesKey in LoadedSequences)
                {
                    sb.AppendLine($"\t{sequencesKey}");
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// List of Sequences.
        /// </summary>
        public static string[] LoadedSequences => Sequences.Keys.ToArray();

        #region Settings
        
        /// <summary>
        /// Flag that is used to force overwriting when creating a sequence
        /// Used in the Deblocker to minimize script length
        /// </summary>
        public static string SequenceCreateOverwrite => $"{ConsoleCoreConfig.CommandFlagPrefix}overwrite";

        /// <summary>
        /// Create Sequence Name
        /// </summary>
        public const string SequenceCreate = "create-sequence";

        /// <summary>
        /// Add To Sequence Name
        /// </summary>
        public const string SequenceAdd = "add-to-sequence";

        /// <summary>
        /// Add Parameter to Sequence Name
        /// </summary>
        public const string SequenceAddParameter = "sequence-add-param";

        /// <summary>
        /// Delete Sequence Name
        /// </summary>
        public const string SequenceDelete = "delete-sequence";

        /// <summary>
        /// Save Sequence Name
        /// </summary>
        public const string SequenceSave = "save-sequence";

        /// <summary>
        /// Load Sequence Name
        /// </summary>
        public const string SequenceLoad = "load-sequence";

        /// <summary>
        /// Run Sequence Name
        /// </summary>
        public const string SequenceRun = "run-sequence";

        /// <summary>
        /// Sequence to Command Name
        /// </summary>
        public const string SequenceToCommand = "sequence-to-command";

        /// <summary>
        /// File to Command Name
        /// </summary>
        public const string FileToCommand = "file-to-command";

        #endregion


        #region Commands

        /// <summary>
        /// Creates a Sequence by name
        /// </summary>
        /// <param name="name">Name of the new Sequence</param>
        /// <param name="overwrite">If true an existing sequence gets overwritten if it exists.</param>
        [Command(
            SequenceCreate,
            HelpMessage = "Creates a Sequence with a specified name",
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            Aliases = new[] { "create-seq" }
        )]
        [OptimizeIL]
        public static void CreateSequence(
            string name, [CommandFlag]
            bool overwrite)
        {
            if (Sequences.ContainsKey(name))
            {
                if (!overwrite)
                {
                    SequenceOut.LogWarning($"A Sequence with the name: {name} does already exist");
                    return;
                }
            }

            Sequences[name] = new Sequence();
        }

        /// <summary>
        /// Adds a parameter to the Specified Sequence
        /// </summary>
        /// <param name="sequence">Sequence Name</param>
        /// <param name="parameterName">Parameter Name</param>
        [Command(
            SequenceAddParameter,
            HelpMessage = "Adds a Parameter to the Sequence",
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            Aliases = new[] { "seq-add-param" }
        )]
        [OptimizeIL]
        public static void AddParameterToSequence(string sequence, string parameterName)
        {
            if (Sequences.ContainsKey(sequence))
            {
                string[] param = parameterName.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < param.Length; i++)
                {
                    Sequences[sequence].Signature.ParameterNames.Add(param[i]);
                }
            }
        }

        /// <summary>
        /// Adds a Command to the Sequence with the Specified Name
        /// </summary>
        /// <param name="name">Name of the Sequene</param>
        /// <param name="command">Command to add</param>
        /// <param name="create">If true the Sequence gets created if it does not exist</param>
        [Command(
            SequenceAdd,
            HelpMessage =
                "Adds a Command to a Sequence. If --create is passed the Sequence will be created if not existing",
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            Aliases = new[] { "add-seq" }
        )]
        [OptimizeIL]
        public static void AddToSequence(
            string name, string command, [CommandFlag]
            bool create)
        {
            if (!Sequences.ContainsKey(name))
            {
                if (!create)
                {
                    SequenceOut.LogWarning($"A Sequence with the name {name} does not exist.");
                    return;
                }

                CreateSequence(name, false);
            }

            string s = command.StartsWith(ConsoleCoreConfig.StringChar.ToString())
                           ? CommandParser.CleanContent(command)
                           : command;

            //SequenceOut.Log("Before: " + command + "\nAfter: " + s);
            Sequences[name].Lines.Add(command);
        }

        /// <summary>
        /// Deletes a Sequence with the specified name.
        /// </summary>
        /// <param name="name">Name of the Sequence</param>
        [Command(
            SequenceDelete,
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            HelpMessage = "Deletes a Sequence by name",
            Aliases = new[] { "delete-seq" }
        )]
        [OptimizeIL]
        public static void DeleteSequence(string name)
        {
            if (!Sequences.ContainsKey(name))
            {
                return;
            }

            Sequences.Remove(name);

            if (!name.StartsWith(DeblockerSettings.BLOCK_NAME_BEGIN))
            {
                SequenceOut.Log("Deleted Sequence: " + name);
            }
        }

        /// <summary>
        /// Deletes all Loaded Sequences.
        /// </summary>
        [Command(
            "clear-sequences",
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            HelpMessage = "Clears all Loaded Sequences",
            Aliases = new[] { "clear-seq" }
        )]
        [OptimizeIL]
        public static void ClearSequences()
        {
            int count = Sequences.Count;
            Sequences.Clear();
            SequenceOut.Log($"{count} Sequences Cleared.");
        }

        /// <summary>
        /// Lists the Specified Sequence Content
        /// </summary>
        /// <param name="name">The Name of the Sequence</param>
        [Command(
            "show-sequence",
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            HelpMessage = "Shows a sequence by name",
            Aliases = new[] { "show-seq" }
        )]
        public static void ShowSequence(string name)
        {
            if (!Sequences.ContainsKey(name))
            {
                SequenceOut.LogWarning($"A Sequence with the name {name} does not exist.");
                return;
            }

            StringBuilder builder = new StringBuilder($"Sequence \"{name}\"\n");
            for (int i = 0; i < Sequences[name].Lines.Count; i++)
            {
                builder.AppendLine($"\t{i}: {Sequences[name].Lines[i]}");
            }

            SequenceOut.Log(builder.ToString());
        }

        /// <summary>
        /// Lists all Loaded Sequences by name
        /// </summary>
        [Command(
            "list-sequences",
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            HelpMessage = "Lists all Loaded Sequence Names",
            Aliases = new[] { "list-seq" }
        )]
        private static void ListSequences()
        {
            SequenceOut.Log(SequenceText);
        }

        /// <summary>
        /// Runs a Sequence by Name
        /// </summary>
        /// <param name="name"></param>
        [Command(
            SequenceRun,
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            HelpMessage = "Runs a sequence by name",
            Aliases = new[] { "run-seq" }
        )]
        [OptimizeIL]
        public static void RunSequence(string name)
        {
            RunSequence(name, "");
        }


        /// <summary>
        /// Runs a Sequence as Async
        /// </summary>
        /// <param name="name"></param>
        [Command(
            "run-seq-async",
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            HelpMessage = "Runs a Sequence in \"background\"."
        )]
        [OptimizeIL]
        public static void RunAsync(string name)
        {
            RunAsync(name, "");
        }

        /// <summary>
        /// Runs a Sequence as Async
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        [Command(
            "run-seq-async",
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            HelpMessage = "Runs a Sequence in \"background\"."
        )]
        [OptimizeIL]
        public static void RunAsync(string name, string parameter)
        {
            if (!Sequences.ContainsKey(name))
            {
                SequenceOut.LogWarning($"A Sequence with the name {name} does not exist.");
                return;
            }

            if (ActiveRoutines.ContainsKey(name))
            {
                //Wait for Active Routine To Finish
                ActiveRoutines[name].OnFinish += () => RunAsync(name, parameter);
            }
            else
            {
                ParameterCollection spc = name.StartsWith(DeblockerSettings.BLOCK_NAME_BEGIN)
                                              ? ParameterCollection.CreateSubCollection(
                                                                                        Sequences[name]
                                                                                            .Signature.ParameterNames
                                                                                            .ToArray(),
                                                                                        parameter
                                                                                       )
                                              : ParameterCollection.CreateCollection(
                                                                                     Sequences[name]
                                                                                         .Signature.ParameterNames
                                                                                         .ToArray(),
                                                                                     parameter
                                                                                    );

                AsyncRunner r = ScriptSystem.MainRunner.Current ?? ScriptSystem.MainRunner.GetCurrent();
                AsyncRunner sub = new AsyncRunner(spc, Sequences[name].Lines.ToArray(), name);
                ActiveRoutines[name] = sub;
                r.AddSub(sub);
            }
        }

        private static void InnerAsyncRun()
        {
            AsyncRunner runner = ScriptSystem.MainRunner.GetCurrent();
            string s = runner?.GetLine();
            if (s != null)
            {
                ParameterCollection.MakeCurrent(runner.Params);
                AConsoleManager.Instance.EnterCommand(s);
                ParameterCollection.MakeCurrent(null);
            }

            if (runner?.Name != null && runner.Finished)
            {
                ScriptSystemInitializer.Logger.Log("Ending Sequence: " + runner.Name);
                ActiveRoutines.Remove(runner.Name);
            }

            ScriptSystem.MainRunner.Clean();
        }


        /// <summary>
        /// Runs a Sequence by Name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        [Command(
            SequenceRun,
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            HelpMessage = "Runs a sequence by name",
            Aliases = new[] { "run-seq" }
        )]
        [OptimizeIL]
        public static void RunSequence(string name, string parameter)
        {
            if (!Sequences.ContainsKey(name))
            {
                SequenceOut.LogWarning($"A Sequence with the name {name} does not exist.");
                return;
            }

            ParameterCollection spc = name.StartsWith(DeblockerSettings.BLOCK_NAME_BEGIN)
                                          ? ParameterCollection.CreateSubCollection(
                                                                                    Sequences[name]
                                                                                        .Signature.ParameterNames
                                                                                        .ToArray(),
                                                                                    parameter
                                                                                   )
                                          : ParameterCollection.CreateCollection(
                                                                                 Sequences[name]
                                                                                     .Signature.ParameterNames
                                                                                     .ToArray(),
                                                                                 parameter
                                                                                );
            foreach (string s in Sequences[name].Lines)
            {
                ParameterCollection.MakeCurrent(spc);
                AConsoleManager.Instance.EnterCommand(s);
            }

            ParameterCollection.MakeCurrent(null);
        }


        /// <summary>
        /// Saves a Sequence By Name to File
        /// </summary>
        /// <param name="name">Name of the Sequence</param>
        /// <param name="file">Destination File</param>
        [Command(
            SequenceSave,
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            HelpMessage = "Saves the Sequence Content to File",
            Aliases = new[] { "save-seq" }
        )]
        public static void SaveSequence(string name, string file)
        {
            if (!Sequences.ContainsKey(name))
            {
                SequenceOut.LogWarning($"A Sequence with the name {name} does not exist.");
                return;
            }

            File.WriteAllLines(file, Sequences[name].Lines);
        }

        /// <summary>
        /// Loads a Sequence from File
        /// </summary>
        /// <param name="name">Name of the new Sequence</param>
        /// <param name="file">Source File</param>
        /// <param name="create">If true will create the Sequence if it does not exist</param>
        [Command(
            SequenceLoad,
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            HelpMessage = "Loads the Sequence Content from File",
            Aliases = new[] { "load-seq" }
        )]
        public static void LoadSequence(
            string name, string file, [CommandFlag]
            bool create)
        {
            if (!File.Exists(file))
            {
                SequenceOut.LogWarning($"The File {file} does not exist.");
                return;
            }

            if (!Sequences.ContainsKey(name))
            {
                if (!create)
                {
                    SequenceOut.LogWarning($"A Sequence with the name {name} does not exist.");
                    return;
                }

                CreateSequence(name, false);
            }
            else
            {
                if (create)
                {
                    CreateSequence(name, true);
                }
            }

            string[] lines = DeblockerCollection.Parse(File.ReadAllText(file)).ToArray();
            string s2 = "";
            foreach (string s1 in lines)
            {
                s2 += s1 + '\n';
            }

            foreach (string s in lines)
            {
                AddToSequence(name, s, false);
            }
        }

        /// <summary>
        /// Creates and Adds a Command that when invoked will run the specified file
        /// </summary>
        /// <param name="fileName">The File that will be run</param>
        /// <param name="commandName">The Command name</param>
        [Command(
            FileToCommand,
            HelpMessage = "Creates and Adds a Command that when invoked will run the specified file",
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            Aliases = new[] { "file-to-cmd" }
        )]
        private static void CreateCommandFromFile(string fileName, string commandName)
        {
            CreateCommandFromFile(fileName, commandName, "Runs File: " + fileName);
        }


        /// <summary>
        /// Creates and Adds a Command that when invoked will run the specified file
        /// </summary>
        /// <param name="fileName">The File that will be run</param>
        /// <param name="commandName">The Command name</param>
        /// <param name="helpText">The Help Text that will be set for this command</param>
        [Command(
            FileToCommand,
            HelpMessage = "Creates and Adds a Command that when invoked will run the specified file",
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            Aliases = new[] { "file-to-cmd" }
        )]
        private static void CreateCommandFromFile(string fileName, string commandName, string helpText)
        {
            Command cmd = new Command(
                                      commandName,
                                      () => ScriptSystem.RunFile(fileName)
                                     );
            cmd.SetHelpMessage(helpText);
            CommandManager.AddCommand(cmd);
        }


        /// <summary>
        /// Creates and Adds a Command that when invoked will run the specified sequence
        /// </summary>
        /// <param name="sequenceName">The Sequence that will be run</param>
        /// <param name="commandName">The Command Name</param>
        [Command(
            SequenceToCommand,
            HelpMessage = "Creates and Adds a Command that when invoked will run the specified sequence",
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            Aliases = new[] { "seq-to-cmd" }
        )]
        private static void CreateCommandFromSequence(string sequenceName, string commandName)
        {
            CreateCommandFromSequence(sequenceName, commandName, "Runs Sequence: " + sequenceName);
        }

        /// <summary>
        /// Creates and Adds a Command that when invoked will run the specified sequence
        /// </summary>
        /// <param name="sequenceName">The Sequence that will be run</param>
        /// <param name="commandName">The Command Name</param>
        /// <param name="helpText">The Help Text that will be set for this command</param>
        [Command(
            SequenceToCommand,
            HelpMessage = "Creates and Adds a Command that when invoked will run the specified sequence",
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            Aliases = new[] { "seq-to-cmd" }
        )]
        private static void CreateCommandFromSequence(string sequenceName, string commandName, string helpText)
        {
            Command cmd = new Command(
                                      commandName,
                                      () => RunSequence(sequenceName)
                                     );
            cmd.SetHelpMessage(helpText);
            CommandManager.AddCommand(cmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="command"></param>
        [Command(
            "for-all",
            Namespace = SEQUENCE_SYSTEM_NAMESPACE,
            HelpMessage = "Runs the specified command for each Item in the List"
        )]
        [OptimizeIL]
        public static void ForAll(string list, string command)
        {
            string[] li = list.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())
                              .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            for (int i = 0; i < li.Length; i++)
            {
                ParameterCollection pc = ParameterCollection.CreateCollection(new[] { "item" }, li[i].Trim());
                ParameterCollection.MakeCurrent(pc);
                AConsoleManager.Instance.EnterCommand(command);
                ParameterCollection.MakeCurrent(null);
            }
        }

        #endregion

    }
}