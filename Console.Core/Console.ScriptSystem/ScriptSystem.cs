using System.Collections.Generic;
using System.IO;
using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.CommandSystem.Builder.IOAutoFill.Files;
using Console.Core.ILOptimizations;
using Console.ScriptSystem.Async;
using Console.ScriptSystem.Deblocker;
using Console.ScriptSystem.Deblocker.Parameters;
using Console.ScriptSystem.Extensions;

namespace Console.ScriptSystem
{
    /// <summary>
    /// Script System Implementation
    /// </summary>
    public static class ScriptSystem
    {
        internal static AsyncRunner MainRunner = new AsyncRunner();

        [Command("extensions-list", "Lists all Extension Filters", "ext-list")]
        private static void ListExtensions()
        {
            ScriptSystemInitializer.Logger.Log("Filters: \n" + AExtensionFilter.FilterList());
        }

        [Command("extensions-add-black", "Adds an Extension to the Blacklist", "ext-add-black")]
        private static void AddExtBlacklist(string ext)
        {
            BlackListFilter.Add(ext);
        }
        [Command("extensions-add-white", "Adds an Extension to the Whitelist", "ext-add-white")]
        private static void AddExtWhitelist(string ext)
        {
            WhiteListFilter.Add(ext);
        }

        [Command("extensions-remove-black", "Removes an Extension from the Blacklist", "ext-rem-black")]
        private static void RemExtBlacklist(string ext)
        {
            BlackListFilter.Remove(ext);
        }
        [Command("extensions-remove-white", "Removes an Extension from the Whitelist", "ext-rem-white")]
        private static void RemExtWhitelist(string ext)
        {
            WhiteListFilter.Remove(ext);
        }

        /// <summary>
        /// Runs a File as Async
        /// </summary>
        /// <param name="file"></param>
        [Command("run-async", "Runs a File in \"background\".")]
        [OptimizeIL]
        public static void RunAsync([FileAutoFill]string file)
        {
            if (File.Exists(file))
            {
                if (!AExtensionFilter.IsAllowed(Path.GetExtension(file)))
                {
                    ScriptSystemInitializer.Logger.LogWarning(
                        $"Can not run Script: {file} because the Extension is not permitted by the current Extension Filter Configuration.");
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
        /// Run Command Name
        /// </summary>
        public const string RunCommandName = "run";

        /// <summary>
        /// Runs a Text File as if it would be typed into the console line by line.
        /// </summary>
        /// <param name="path">Filepath</param>
        [Command(RunCommandName, "Run a  file.")]
        [OptimizeIL]
        public static void RunFile([FileAutoFill]string path)
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