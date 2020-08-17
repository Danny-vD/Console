using System.IO;
using System.Linq;
using Console.Core;
using Console.Core.CommandSystem;

namespace Console.ScriptSystem
{
    /// <summary>
    /// Script System Implementation
    /// </summary>
    public static class ScriptSystem
    {
        /// <summary>
        /// Runs a Text File as if it would be typed into the console line by line.
        /// </summary>
        /// <param name="path">Filepath</param>
        [Command("run", "Run a  file.")]
        public static void RunFile(string path)
        {
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path).Where(x=>!string.IsNullOrEmpty(x.Trim())).ToArray();
                foreach (string line in lines)
                {
                    AConsoleManager.Instance.EnterCommand(line);
                }
            }
            else
            {
                AConsoleManager.Instance.LogWarning("File does not exist: " + path);
            }
        }
    }
}