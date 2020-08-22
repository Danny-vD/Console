using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.CommandSystem.Commands;
using Console.Core.LogSystem;
using Console.ScriptSystem.Deblocker;
using Console.ScriptSystem.Deblocker.Parameters;

namespace Console.ScriptSystem
{
    /// <summary>
    /// Static SequenceSystem API
    /// </summary>
    public static class SequenceSystem
    {
        private static ALogger SequenceOut = TypedLogger.CreateTypedWithPrefix("SequenceSystem");

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

        /// <summary>
        /// Flag that is used to force overwriting when creating a sequence
        /// Used in the Deblocker to minimize script length
        /// </summary>
        public const string SequenceCreateOverwrite = "-overwrite";

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

        /// <summary>
        /// Internal Dictionary of Sequences.
        /// </summary>
        private static readonly Dictionary<string, Sequence> Sequences = new Dictionary<string, Sequence>();


        /// <summary>
        /// Creates a Sequence by name
        /// </summary>
        /// <param name="name">Name of the new Sequence</param>
        /// <param name="overwrite">If true an existing sequence gets overwritten if it exists.</param>
        [Command(SequenceCreate, "Creates a Sequence with a specified name", "create-seq")]
        public static void CreateSequence(string name, [CommandFlag] bool overwrite)
        {
            if (Sequences.ContainsKey(name))
            {
                SequenceOut.LogWarning($"A Sequence with the name: {name} does already exist");
                if (!overwrite) return;
            }
            Sequences[name] = new Sequence();
        }

        /// <summary>
        /// Adds a parameter to the Specified Sequence
        /// </summary>
        /// <param name="sequence">Sequence Name</param>
        /// <param name="parameterName">Parameter Name</param>
        [Command(SequenceAddParameter, "Adds a Parameter to the Sequence", "seq-add-param")]
        private static void AddParameterToSequence(string sequence, string parameterName)
        {
            if (Sequences.ContainsKey(sequence))
            {
                Sequences[sequence].Signature.ParameterNames.Add(parameterName);
            }
        }

        /// <summary>
        /// Adds a Command to the Sequence with the Specified Name
        /// </summary>
        /// <param name="name">Name of the Sequene</param>
        /// <param name="command">Command to add</param>
        /// <param name="create">If true the Sequence gets created if it does not exist</param>
        [Command(SequenceAdd, "Adds a Command to a Sequence. If -create is passed the Sequence will be created if not existing", "add-seq")]
        public static void AddToSequence(string name, string command, [CommandFlag] bool create)
        {
            if (!Sequences.ContainsKey(name))
            {
                SequenceOut.LogWarning($"A Sequence with the name {name} does not exist.");
                if (!create)
                    return;
                CreateSequence(name, false);
            }
            string s = command.StartsWith(ConsoleCoreConfig.StringChar.ToString()) ? CommandParser.CleanContent(command) : command;

            SequenceOut.Log("Before: " + command + "\nAfter: " + s);
            Sequences[name].Lines.Add(command);
        }

        /// <summary>
        /// Deletes a Sequence with the specified name.
        /// </summary>
        /// <param name="name">Name of the Sequence</param>
        [Command(SequenceDelete, "Deletes a Sequence by name", "delete-seq")]
        public static void DeleteSequence(string name)
        {
            if (!Sequences.ContainsKey(name))
            {
                return;
            }
            Sequences.Remove(name);

            SequenceOut.Log("Deleted Sequence: " + name);
        }

        [Command("clear-sequences", "Clears all Loaded Sequences", "clear-seq")]
        public static void ClearSequences()
        {
            Sequences.Clear();
            SequenceOut.Log($"Sequences Cleared.");
        }

        /// <summary>
        /// Lists the Specified Sequence Content
        /// </summary>
        /// <param name="name">The Name of the Sequence</param>
        [Command("show-sequence", "Shows a sequence by name", "show-seq")]
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
            ScriptSystemInitializer.Logger.Log(builder.ToString());
        }

        /// <summary>
        /// Lists all Loaded Sequences by name
        /// </summary>
        [Command("list-sequences", "Lists all Loaded Sequence Names", "list-seq")]
        private static void ListSequences() => ScriptSystemInitializer.Logger.LogWarning(SequenceText);

        /// <summary>
        /// Runs a Sequence by Name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        [Command(SequenceRun, "Runs a sequence by name", "run-seq")]
        public static void RunSequence(string name) => RunSequence(name, "");
        /// <summary>
        /// Runs a Sequence by Name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        [Command(SequenceRun, "Runs a sequence by name", "run-seq")]
        public static void RunSequence(string name, string parameter)
        {
            if (!Sequences.ContainsKey(name))
            {
                SequenceOut.LogWarning($"A Sequence with the name {name} does not exist.");
                return;
            }
            ParameterCollection spc = name.StartsWith("BLOCK_") ? 
                ParameterCollection.CreateSubCollection(Sequences[name].Signature.ParameterNames.ToArray(), parameter) :
                ParameterCollection.CreateCollection(Sequences[name].Signature.ParameterNames.ToArray(), parameter);
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
        [Command(SequenceSave, "Saves the Sequence Content to File", "save-seq")]
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
        [Command(SequenceLoad, "Loads the Sequence Content from File", "load-seq")]
        public static void LoadSequence(string name, string file, [CommandFlag] bool create)
        {
            if (!File.Exists(file))
            {
                SequenceOut.LogWarning($"The File {file} does not exist.");
                return;
            }
            if (!Sequences.ContainsKey(name))
            {
                SequenceOut.LogWarning($"A Sequence with the name {name} does not exist.");
                if (!create)
                    return;
                CreateSequence(name, false);
            }
            else
            {
                if (!create)
                    return;
                CreateSequence(name, true);

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
        [Command(FileToCommand, "Creates and Adds a Command that when invoked will run the specified file", "file-to-cmd")]
        private static void CreateCommandFromFile(string fileName, string commandName) =>
            CreateCommandFromFile(fileName, commandName, "Runs File: " + fileName);


        /// <summary>
        /// Creates and Adds a Command that when invoked will run the specified file
        /// </summary>
        /// <param name="fileName">The File that will be run</param>
        /// <param name="commandName">The Command name</param>
        /// <param name="helpText">The Help Text that will be set for this command</param>
        [Command(FileToCommand, "Creates and Adds a Command that when invoked will run the specified file", "file-to-cmd")]
        private static void CreateCommandFromFile(string fileName, string commandName, string helpText)
        {
            Command cmd = new Command(commandName,
                () => ScriptSystem.RunFile(fileName));
            cmd.SetHelpMessage(helpText);
            CommandManager.AddCommand(cmd);
        }


        /// <summary>
        /// Creates and Adds a Command that when invoked will run the specified sequence
        /// </summary>
        /// <param name="sequenceName">The Sequence that will be run</param>
        /// <param name="commandName">The Command Name</param>
        [Command(SequenceToCommand, "Creates and Adds a Command that when invoked will run the specified sequence", "seq-to-cmd")]
        private static void CreateCommandFromSequence(string sequenceName, string commandName) =>
            CreateCommandFromSequence(sequenceName, commandName, "Runs Sequence: " + sequenceName);

        /// <summary>
        /// Creates and Adds a Command that when invoked will run the specified sequence
        /// </summary>
        /// <param name="sequenceName">The Sequence that will be run</param>
        /// <param name="commandName">The Command Name</param>
        /// <param name="helpText">The Help Text that will be set for this command</param>
        [Command(SequenceToCommand, "Creates and Adds a Command that when invoked will run the specified sequence", "seq-to-cmd")]
        private static void CreateCommandFromSequence(string sequenceName, string commandName, string helpText)
        {
            Command cmd = new Command(commandName,
                () => RunSequence(sequenceName));
            cmd.SetHelpMessage(helpText);
            CommandManager.AddCommand(cmd);
        }

        [Command("for-all", "Runs the specified command for each Item in the List")]
        private static void ForAll(string list, string command)
        {
            string[] li = list.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            for (int i = 0; i < li.Length; i++)
            {
                ParameterCollection pc = ParameterCollection.CreateCollection(new[] { "item" }, li[i].Trim());
                ParameterCollection.MakeCurrent(pc);
                AConsoleManager.Instance.EnterCommand(command);
                ParameterCollection.MakeCurrent(null);
            }
        }
    }
}