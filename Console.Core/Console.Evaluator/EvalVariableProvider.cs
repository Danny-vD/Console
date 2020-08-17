using System;
using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.PropertySystem;
using Console.EnvironmentVariables;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.OPCodes;

namespace Console.Evaluator
{
    /// <summary>
    /// VariableContainer Implementation with the name "eval" to be used in the EnvironmentVariable Extension.
    /// Evaluates the Expression between the Brackets $eval(MYEXPRESSION)
    /// </summary>
    public class EvalVariableProvider : VariableContainer
    {
        /// <summary>
        /// The Evaluator Syntax. Possible Values: CSharp and VisualBasic
        /// </summary>
        [Property("evaluator.syntax")]
        private static ParserSyntax Syntax = ParserSyntax.CSharp;

        /// <summary>
        /// Specifies if the Evaluator is Case Sensitive
        /// </summary>
        [Property("evaluator.casesensitive")]
        private static bool CaseSensitive;

        /// <summary>
        /// Specifies if the Evaluator should throw an Exception when the Expression is Faulty.
        /// </summary>
        [Property("evaluator.exceptions.raise")]
        private static bool RaiseExceptions = true;

        /// <summary>
        /// The Evaluator instance
        /// </summary>
        private Core.Evaluator eval;

        /// <summary>
        /// Public Constructor.
        /// </summary>
        public EvalVariableProvider() : base("eval")
        {
            CommandAttributeUtils.AddCommands(this);
            EnvironmentVariableManager.AddProvider(this);
        }
        /// <summary>
        /// Returns the Result of the Specified Expression
        /// </summary>
        /// <param name="parameter">The Expression</param>
        /// <returns>Result of the Expression</returns>
        public override string GetValue(string parameter)
        {
            if (Providers.ContainsKey(parameter))
            {
                return Providers[parameter].GetValue(parameter);
            }
            Setup();

            OPCode ret;
            try
            {
                ret = eval.Parse(parameter);
            }
            catch (Exception e)
            {
                if (RaiseExceptions)
                    throw;
                return "EVAL_ERROR";
            }
            return ret.Value.ToString();
        }

        /// <summary>
        /// Initializes the Evaluator Instance
        /// </summary>
        private void Setup()
        {
            if (eval == null)
            {
                eval = new Core.Evaluator(Syntax, CaseSensitive);
                eval.RaiseVariableNotFoundException = RaiseExceptions;
            }
        }

        /// <summary>
        /// Adds Variables/Functions of the specified type as usable functions in the evaluator.
        /// </summary>
        /// <param name="qualifiedName">Assembly Qualified Name of the Type</param>
        [Command("eval-add-var", "Adds a variable/function container to the evaluator")]
        public void AddVariable(string qualifiedName)
        {
            Setup();

            object obj = null;
            try
            {
                obj = Activator.CreateInstance(Type.GetType(qualifiedName));
            }
            catch (Exception e)
            {
                AConsoleManager.Instance.LogWarning("Can not Create instance of type: " + qualifiedName);
                return;
            }

            eval.AddEnvironmentFunctions(obj);
        }

        /// <summary>
        /// Remove all Variables and Functions of the selected type.
        /// </summary>
        /// <param name="qualifiedName">Assembly Qualified Name of the Type</param>
        [Command("eval-rem-var", "Removes a variable/function container from the evaluator")]
        public void RemoveVariable(string qualifiedName)
        {
            if (eval == null) return;
            for (int i = eval.mEnvironmentFunctionsList.Count - 1; i >= 0; i--)
            {
                if (eval.mEnvironmentFunctionsList[i] != null &&
                    eval.mEnvironmentFunctionsList[i].GetType().AssemblyQualifiedName == qualifiedName)
                {
                    eval.mEnvironmentFunctionsList.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// Clears all Variables and Functions from the Evaluator.
        /// </summary>
        [Command("eval-clear-var", "Clears all Variable/Function Container from the Evaluator")]
        public void ClearVariables()
        {
            eval?.mEnvironmentFunctionsList.Clear();
        }
    }
}