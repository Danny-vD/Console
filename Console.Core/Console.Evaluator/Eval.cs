using System;

using Console.Core.CommandSystem.Attributes;
using Console.Core.PropertySystem;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.OPCodes;

namespace Console.Evaluator
{
    /// <summary>
    /// 
    /// </summary>
    public static class Eval
    {

        /// <summary>
        /// 
        /// </summary>
        public const string EVAL_NAMESPACE = "evaluator";

        private static ParserSyntax _syntax = ParserSyntax.CSharp;

        private static bool _caseSensitive;

        /// <summary>
        /// Specifies if the Evaluator should throw an Exception when the Expression is Faulty.
        /// </summary>
        [Property("evaluator.exceptions.raise")]
        public static readonly bool RaiseExceptions = true;

        /// <summary>
        /// The Evaluator instance
        /// </summary>
        private static Core.Evaluator eval;
        /// <summary>
        /// The Evaluator Syntax. Possible Values: CSharp and VisualBasic
        /// </summary>
        [Property("evaluator.syntax")]
        public static ParserSyntax Syntax
        {
            get => _syntax;
            set
            {
                if (value == _syntax)
                {
                    return;
                }

                _syntax = value;
                eval = null;
                Setup();
            }
        }

        /// <summary>
        /// Specifies if the Evaluator is Case Sensitive
        /// </summary>
        [Property("evaluator.casesensitive")]
        public static bool CaseSensitive
        {
            get => _caseSensitive;
            set
            {
                if (value == _caseSensitive)
                {
                    return;
                }

                _caseSensitive = value;
                eval = null;
                Setup();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static OPCode Parse(string expr)
        {
            Setup();
            return eval.Parse(expr);
        }

        /// <summary>
        /// Initializes the Evaluator Instance
        /// </summary>
        private static void Setup()
        {
            if (eval == null)
            {
                eval = new Core.Evaluator(Syntax, CaseSensitive) { RaiseVariableNotFoundException = RaiseExceptions };
            }
        }

        /// <summary>
        /// Adds Variables/Functions of the specified type as usable functions in the evaluator.
        /// </summary>
        /// <param name="qualifiedName">Assembly Qualified Name of the Type</param>
        [Command(
            "eval-add-var",
            HelpMessage = "Adds a variable/function container to the evaluator",
            Namespace = EVAL_NAMESPACE
        )]
        public static void AddVariable(string qualifiedName)
        {
            Setup();

            object obj = null;
            try
            {
                obj = Activator.CreateInstance(Type.GetType(qualifiedName));
            }
            catch (Exception)
            {
                EvalInitializer.Logger.LogWarning("Can not Create instance of type: " + qualifiedName);
                return;
            }

            eval.AddEnvironmentFunctions(obj);
        }



        /// <summary>
        /// Remove all Variables and Functions of the selected type.
        /// </summary>
        /// <param name="qualifiedName">Assembly Qualified Name of the Type</param>
        [Command(
            "eval-rem-var",
            HelpMessage = "Removes a variable/function container from the evaluator",
            Namespace = EVAL_NAMESPACE
        )]
        public static void RemoveVariable(string qualifiedName)
        {
            Setup();
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
        [Command(
            "eval-clear-var",
            HelpMessage = "Clears all Variable/Function Container from the Evaluator",
            Namespace = EVAL_NAMESPACE
        )]
        public static void ClearVariables()
        {
            eval?.mEnvironmentFunctionsList.Clear();
        }


    }
}