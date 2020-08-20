using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Console.Core;
using Console.Core.CommandSystem;
using Console.ScriptSystem.Deblocker;

namespace Console.ScriptSystem
{
    /// <summary>
    /// Static SequenceSystem API
    /// </summary>
    public static class SequenceSystem
    {
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
        /// Internal Dictionary of Sequences.
        /// </summary>
        private static readonly Dictionary<string, List<string>> Sequences = new Dictionary<string, List<string>>();

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
                //AConsoleManager.Instance.LogWarning($"A Sequence with the name: {name} does already exist");
                if (!overwrite) return;
            }
            Sequences[name] = new List<string>();
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
                //AConsoleManager.Instance.LogWarning($"A Sequence with the name {name} does not exist.");
                if (!create)
                    return;
                CreateSequence(name, false);
            }
            //string s = command.StartsWith(ConsoleCoreConfig.StringChar.ToString()) ? CommandParser.CleanContent(command) : command;

            //AConsoleManager.Instance.Log("Before: " + command + "\nAfter: " + s);
            Sequences[name].Add(command);
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

            //AConsoleManager.Instance.Log("Deleted Sequence: " + name);
        }

        [Command("clear-sequences", "Clears all Loaded Sequences", "clear-seq")]
        public static void ClearSequences()
        {
            Sequences.Clear();
            AConsoleManager.Instance.Log($"Sequences Cleared.");
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
                AConsoleManager.Instance.LogWarning($"A Sequence with the name {name} does not exist.");
                return;
            }
            StringBuilder builder = new StringBuilder($"Sequence \"{name}\"\n");
            for (int i = 0; i < Sequences[name].Count; i++)
            {
                builder.AppendLine($"\t{i}: {Sequences[name][i]}");
            }
            AConsoleManager.Instance.Log(builder.ToString());
        }

        /// <summary>
        /// Lists all Loaded Sequences by name
        /// </summary>
        [Command("list-sequences", "Lists all Loaded Sequence Names", "list-seq")]
        public static void ListSequences()
        {
            StringBuilder sb = new StringBuilder("Sequences:\n");
            foreach (string sequencesKey in Sequences.Keys)
            {
                sb.AppendLine($"\t{sequencesKey}");
            }
            AConsoleManager.Instance.Log(sb.ToString());
        }

        /// <summary>
        /// Runs a Sequence by Name
        /// </summary>
        /// <param name="name"></param>
        [Command(SequenceRun, "Runs a sequence by name", "run-seq")]
        public static void RunSequence(string name)
        {
            if (!Sequences.ContainsKey(name))
            {
                AConsoleManager.Instance.LogWarning($"A Sequence with the name {name} does not exist.");
                return;
            }
            foreach (string s in Sequences[name])
            {
                AConsoleManager.Instance.EnterCommand(s);
            }
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
                AConsoleManager.Instance.LogWarning($"A Sequence with the name {name} does not exist.");
                return;
            }
            File.WriteAllLines(file, Sequences[name]);
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
                AConsoleManager.Instance.LogWarning($"The File {file} does not exist.");
                return;
            }
            if (!Sequences.ContainsKey(name))
            {
                AConsoleManager.Instance.LogWarning($"A Sequence with the name {name} does not exist.");
                if (!create)
                    return;
                CreateSequence(name, false);
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
    }
}