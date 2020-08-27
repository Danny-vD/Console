using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.PropertySystem;
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

        public class AsyncRunner
        {
            static AsyncRunner() => PropertyAttributeUtils.AddProperties<AsyncRunner>();

            public enum ScriptScopeSettings
            {
                /// <summary>
                /// Will be executed as Subscript of the Parent
                /// </summary>
                SubLevel,
                /// <summary>
                /// Will be executed as "Root" Script.
                /// </summary>
                MainLevel,
                /// <summary>
                /// Will be executed on the Same Level as the Parent Script
                /// </summary>
                ParentLevel
            }

            public enum ScriptExecutionOrder
            {
                /// <summary>
                /// Will Execute the Parent Script Content before Executing the Subscripts
                /// </summary>
                ParentFirst,
                /// <summary>
                /// Will Execute the Sub Scripts before continuing the Parent Script
                /// </summary>
                SubFirst
            }

            public enum SubScriptOrder
            {
                /// <summary>
                /// The Last Script that got Inserted at this level will be finished first
                /// </summary>
                Stack,

                /// <summary>
                /// The First Script that got Inserted at this level will be finished first
                /// </summary>
                Queue,

                /// <summary>
                /// The Subscripts will get one execution cycle in turns
                /// </summary>
                Rotating
            }

            private readonly List<AsyncRunner> SubRunners = new List<AsyncRunner>();
            private AsyncRunner Parent;
            private int SubRunnerPosition;
            private readonly string[] RunnerContent;
            private int RunnerPosition;

            public ParameterCollection Params { get; }
            public AsyncRunner Current { get; private set; }
            public bool SubsFinished => SubRunners.All(x => x.Finished);
            public bool ThisFinished => RunnerPosition >= RunnerContent.Length;
            public bool Finished => ThisFinished && SubsFinished;
            [Property("scriptsystem.async.scope")]
            public static ScriptScopeSettings ScopeSettings = ScriptScopeSettings.SubLevel;
            [Property("scriptsystem.async.executionorder")]
            public static ScriptExecutionOrder ExecutionOrder = ScriptExecutionOrder.SubFirst;
            [Property("scriptsystem.async.suborder")]
            public static SubScriptOrder SubOrder = SubScriptOrder.Rotating;

            public AsyncRunner() : this(ParameterCollection.CreateCollection(new string[0], ""))
            {
            }

            public AsyncRunner(ParameterCollection param) : this(param, new string[0])
            {
            }

            public AsyncRunner(ParameterCollection param, string[] lines)
            {
                RunnerContent = lines;
                Params = param;
            }

            public void AddSub(AsyncRunner runner)
            {
                if (ScopeSettings == ScriptScopeSettings.MainLevel)
                {
                    MainRunner.InnerAddSub(runner);
                }
                if (ScopeSettings == ScriptScopeSettings.SubLevel)
                {
                    InnerAddSub(runner);
                }
                if (ScopeSettings == ScriptScopeSettings.ParentLevel)
                {
                    if (Parent == null) InnerAddSub(runner);
                    else Parent.InnerAddSub(runner);
                }
            }

            private void InnerAddSub(AsyncRunner runner)
            {
                SubRunners.Add(runner);
                runner.SetParent(this);
            }

            private void SetParent(AsyncRunner runner)
            {
                Parent = runner;
            }

            private AsyncRunner GetNextSub()
            {
                if (SubOrder == SubScriptOrder.Rotating)
                {
                    if (SubRunnerPosition >= SubRunners.Count) SubRunnerPosition = 0;
                    AsyncRunner ret = SubRunners[SubRunnerPosition];
                    SubRunnerPosition++;
                    return ret;
                }
                if (SubOrder == SubScriptOrder.Queue)
                {
                    return SubRunners[0];
                }
                if (SubOrder == SubScriptOrder.Stack)
                {
                    return SubRunners[SubRunners.Count - 1];
                }
                return null;
            }

            public AsyncRunner GetCurrent()
            {
                if (ExecutionOrder == ScriptExecutionOrder.SubFirst)
                {
                    if (!SubsFinished)
                        Current = GetNextSub().GetCurrent();
                    else if (!ThisFinished)
                        Current = this;
                    return Current ?? MainRunner;
                }
                else if (ExecutionOrder == ScriptExecutionOrder.ParentFirst)
                {
                    if (!ThisFinished)
                        Current = this;
                    else if (!SubsFinished)
                        Current = GetNextSub().GetCurrent();
                    return Current ?? MainRunner;
                }
                return MainRunner;
            }

            public string GetLine()
            {
                if (ThisFinished) return null;
                string r = RunnerContent[RunnerPosition];
                RunnerPosition++;
                return r;
            }

            public void Clean()
            {
                for (int i = SubRunners.Count - 1; i >= 0; i--)
                {
                    AsyncRunner asyncRunner = SubRunners[i];
                    if (asyncRunner.Finished)
                    {
                        if (Current == asyncRunner) Current = asyncRunner.Parent;
                        SubRunners.RemoveAt(i);
                    }
                    else
                        asyncRunner.Clean();
                }
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