using System.Collections.Generic;
using System.IO;

using Console.Core;
using Console.Core.CommandSystem.Attributes;
using Console.Core.ILOptimizations;
using Console.Script.Async;
using Console.Script.Deblocker;
using Console.Script.Deblocker.Parameters;
using Console.Script.Extensions;

namespace Console.Script
{
    /// <summary>
    /// Script System Implementation
    /// </summary>
    public static class ScriptSystem
    {

        /// <summary>
        /// Run Command Name
        /// </summary>
        public const string RunCommandName = "run";

        internal static AsyncRunner MainRunner = new AsyncRunner();

        [Command(
            "extensions-list",
            Namespace = ScriptSystemInitializer.SCRIPT_SYSTEM_NAMESPACE,
            HelpMessage = "Lists all Extension Filters",
            Aliases = new[] { "ext-list" }
        )]
        private static void ListExtensions()
        {
            ScriptSystemInitializer.Logger.Log("Filters: \n" + AExtensionFilter.FilterList());
        }

        [Command(
            "extensions-add-black",
            HelpMessage = "Adds an Extension to the Blacklist",
            Namespace = ScriptSystemInitializer.SCRIPT_SYSTEM_NAMESPACE,
            Aliases = new[] { "ext-add-black" }
        )]
        private static void AddExtBlacklist(string ext)
        {
            BlackListFilter.Add(ext);
        }

        [Command(
            "extensions-add-white",
            HelpMessage = "Adds an Extension to the Whitelist",
            Namespace = ScriptSystemInitializer.SCRIPT_SYSTEM_NAMESPACE,
            Aliases = new[] { "ext-add-white" }
        )]
        private static void AddExtWhitelist(string ext)
        {
            WhiteListFilter.Add(ext);
        }

        [Command(
            "extensions-remove-black",
            HelpMessage = "Removes an Extension from the Blacklist",
            Namespace = ScriptSystemInitializer.SCRIPT_SYSTEM_NAMESPACE,
            Aliases = new[] { "ext-rem-black" }
        )]
        private static void RemExtBlacklist(string ext)
        {
            BlackListFilter.Remove(ext);
        }

        [Command(
            "extensions-remove-white",
            HelpMessage = "Removes an Extension from the Whitelist",
            Namespace = ScriptSystemInitializer.SCRIPT_SYSTEM_NAMESPACE,
            Aliases = new[] { "ext-rem-white" }
        )]
        private static void RemExtWhitelist(string ext)
        {
            WhiteListFilter.Remove(ext);
        }

        /// <summary>
        /// Runs a File as Async
        /// </summary>
        /// <param name="file"></param>
        [Command(
            "run-async",
            Namespace = ScriptSystemInitializer.SCRIPT_SYSTEM_NAMESPACE,
            HelpMessage = "Runs a File in \"background\"."
        )]
        [OptimizeIL]
        public static void RunAsync(string file)
        {
            if (File.Exists(file))
            {
                if (!AExtensionFilter.IsAllowed(Path.GetExtension(file)))
                {
                    ScriptSystemInitializer.Logger.LogWarning(
                                                              $"Can not run Script: {file} because the Extension is not permitted by the current Extension Filter Configuration."
                                                             );
                    return;
                }

                List<string> lines = DeblockerCollection.Parse(File.ReadAllText(file));
                AsyncRunner r = MainRunner.Current ?? MainRunner.GetCurrent();
                r.AddSub(new AsyncRunner(ParameterCollection.CreateCollection(new string[0], ""), lines.ToArray()));
            }
            else
            {
                ScriptSystemInitializer.Logger.LogWarning("File does not exist: " + file);
            }
        }

        /// <summary>
        /// Runs a Text File as if it would be typed into the console line by line.
        /// </summary>
        /// <param name="path">Filepath</param>
        [Command(
            RunCommandName,
            Namespace = ScriptSystemInitializer.SCRIPT_SYSTEM_NAMESPACE,
            HelpMessage = "Run a  file."
        )]
        [OptimizeIL]
        public static void RunFile(string path)
        {
            if (File.Exists(path))
            {
                List<string> lines = DeblockerCollection.Parse(File.ReadAllText(path));
                foreach (string line in lines)
                {
                    AConsoleManager.Instance.EnterCommand(line);
                }
            }
            else
            {
                ScriptSystemInitializer.Logger.LogWarning("File does not exist: " + path);
            }
        }

    }
}