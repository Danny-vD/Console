using System.Collections.Generic;
using System.IO;
using Console.Core;
using Console.Core.CommandSystem;
using Console.ScriptSystem.Async;
using Console.ScriptSystem.Deblocker;
using Console.ScriptSystem.Deblocker.Parameters;

namespace Console.ScriptSystem
{
    /// <summary>
    /// Script System Implementation
    /// </summary>
    public static class ScriptSystem
    {
        internal static AsyncRunner MainRunner = new AsyncRunner();

        [Command("run-async", "Runs a File in \"background\".")]
        private static void RunAsync(string file)
        {
            if (File.Exists(file))
            {
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