﻿using System.IO;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;

namespace Console.ScriptSystem
{
    public class ScriptSystem
    {
        private ScriptSystem() { }

        [Command("run", "Run a  file.")]
        public static void RunFile(string path)
        {
            if (File.Exists(path))
            {
                AConsoleManager.Instance.Log("Executing Program..");
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    AConsoleManager.Instance.EnterCommand(line);
                }
            }
            else
            {
                AConsoleManager.Instance.LogWarning("File does not exist.");
            }
        }
    }
}