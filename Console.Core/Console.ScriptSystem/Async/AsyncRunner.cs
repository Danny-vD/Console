using System;
using System.Collections.Generic;
using System.Linq;
using Console.Core.PropertySystem;
using Console.ScriptSystem.Deblocker.Parameters;

namespace Console.ScriptSystem.Async
{
    /// <summary>
    /// Class that Implements Async Script Sequences
    /// </summary>
    public class AsyncRunner
    {

        /// <summary>
        /// Static Constructor
        /// </summary>
        static AsyncRunner()
        {
            PropertyAttributeUtils.AddProperties<AsyncRunner>();
        }



        private readonly List<AsyncRunner> SubRunners = new List<AsyncRunner>();
        /// <summary>
        /// The Parent of this Async Runner
        /// </summary>
        public AsyncRunner Parent { get; private set; }
        private int SubRunnerPosition;
        private readonly string[] RunnerContent;
        private int RunnerPosition;
        /// <summary>
        /// Gets Invoked when the AsyncRunner finishes executing the Content
        /// </summary>
        public event Action OnFinish;
        /// <summary>
        /// The Name of the AsyncRunner. This is used to Avoid Unpredictable Results when executing the same content at the same time.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The Parameter Collection of this Runner
        /// </summary>
        public ParameterCollection Params { get; }

        /// <summary>
        /// The Current Runner that is used
        /// </summary>
        public AsyncRunner Current { get; private set; }
        /// <summary>
        /// True if all Subrunners Finished Executing
        /// </summary>
        public bool SubsFinished => SubRunners.All(x => x.Finished);
        /// <summary>
        /// True if this Runner finished executing its content
        /// </summary>
        public bool ThisFinished => RunnerPosition >= RunnerContent.Length;
        /// <summary>
        /// True if the Runner and All SubRunners Finished Executing
        /// </summary>
        public bool Finished => ThisFinished && SubsFinished;

        /// <summary>
        /// Sets the Position where new SubRunner get added.
        /// SubLevel => as Sub of the Runner that started the Sequence
        /// </summary>
        [Property("scriptsystem.async.scope")]
        public static ScriptScopeSettings ScopeSettings = ScriptScopeSettings.SubLevel;

        /// <summary>
        /// Sets the Order in which the Runners Get Executed
        /// SubFirst => Sub Runners have Higher Priority than the ParentRunner
        /// </summary>
        [Property("scriptsystem.async.executionorder")]
        public static ScriptExecutionOrder ExecutionOrder = ScriptExecutionOrder.SubFirst;

        /// <summary>
        /// Sets the Order in which the SubRunners get executed.
        /// </summary>
        [Property("scriptsystem.async.suborder")]
        public static SubScriptOrder SubOrder = SubScriptOrder.Rotating;


        /// <summary>
        /// Public Constructor
        /// </summary>
        public AsyncRunner() : this(ParameterCollection.CreateCollection(new string[0], ""))
        {
        }


        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="param">The Parameter Collection of the Runner</param>
        public AsyncRunner(ParameterCollection param) : this(param, new string[0])
        {
        }

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="param">The Parameter Collection of the Runner</param>
        /// <param name="lines">The Content of the Runner</param>
        /// <param name="name">Name of the Async Runner.(Used to manage Async Execution of the Same Content</param>
        public AsyncRunner(ParameterCollection param, string[] lines, string name = null)
        {
            Name = name;
            RunnerContent = lines;
            Params = param;
        }

        /// <summary>
        /// Adds a Sub Runner to this runner
        /// </summary>
        /// <param name="runner">Runner to add</param>
        public void AddSub(AsyncRunner runner)
        {
            if (ScopeSettings == ScriptScopeSettings.SubLevel)
            {
                InnerAddSub(runner);
            }
            if (ScopeSettings == ScriptScopeSettings.ParentLevel)
            {
                if (Parent == null)
                {
                    InnerAddSub(runner);
                }
                else
                {
                    Parent.AddSub(runner);
                }
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

        /// <summary>
        /// Returns the Next SubRunner of this Runner
        /// </summary>
        /// <returns></returns>
        private AsyncRunner GetNextSub()
        {
            if (SubOrder == SubScriptOrder.Rotating)
            {
                if (SubRunnerPosition >= SubRunners.Count)
                {
                    SubRunnerPosition = 0;
                }
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

        /// <summary>
        /// Returns the Runner that should be executed next
        /// </summary>
        /// <returns></returns>
        public AsyncRunner GetCurrent()
        {
            if (ExecutionOrder == ScriptExecutionOrder.SubFirst)
            {
                if (!SubsFinished)
                {
                    return GetNextSub().GetCurrent();
                }
                else if (!ThisFinished)
                {
                    return this;
                }
                return ScriptSystem.MainRunner;
            }
            else if (ExecutionOrder == ScriptExecutionOrder.ParentFirst)
            {
                if (!ThisFinished)
                {
                    return this;
                }
                else if (!SubsFinished)
                {
                    return GetNextSub().GetCurrent();
                }
                return ScriptSystem.MainRunner;
            }
            return ScriptSystem.MainRunner;
        }

        /// <summary>
        /// Returns the Next Line of this Runner
        /// NULL if ThisFinished is true
        /// </summary>
        /// <returns>Next Line</returns>
        public string GetLine()
        {
            if (ThisFinished)
            {
                return null;
            }
            string r = RunnerContent[RunnerPosition];
            RunnerPosition++;
            return r;
        }

        /// <summary>
        /// Removes all Finished Runners
        /// </summary>
        public void Clean()
        {
            for (int i = SubRunners.Count - 1; i >= 0; i--)
            {
                AsyncRunner asyncRunner = SubRunners[i];
                if (asyncRunner.Finished)
                {
                    //Invoke OnFinish Event
                    asyncRunner.OnFinish?.Invoke();

                    //Check the Finished State Again to make sure that we dont accidentially throw away code that is still meant to be executed.
                    if (!asyncRunner.Finished) continue;

                    if (Current == asyncRunner)
                    {
                        Current = asyncRunner.Parent;
                    }
                    SubRunners.Remove(asyncRunner);
                }
                else
                {
                    asyncRunner.Clean();
                }
            }
        }
    }
}