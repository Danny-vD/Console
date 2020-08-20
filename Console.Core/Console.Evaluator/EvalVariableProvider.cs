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

        [Command("if", "If the Expression is evaluated to true the command is executed.")]
        private void ConditionalIf(string expr, string command)
        {
            Setup();
            OPCode ret = eval.Parse(expr);
            if (ret != null && ret.CanReturn(EvalType.Boolean))
            {
                bool r = (bool)ret.Value;
                if (r)
                {
                    AConsoleManager.Instance.EnterCommand(command);
                }
            }
        }
        [Command("ifelse", "Invokes a command based on the expression.")]
        private void IfElse(string expr, string commandIfTrue, string commandIfFalse)
        {
            Setup();
            OPCode ret = eval.Parse(expr);
            if (ret != null && ret.CanReturn(EvalType.Boolean))
            {
                bool r = (bool)ret.Value;
                if (r)
                {
                    AConsoleManager.Instance.EnterCommand(commandIfTrue);
                }
                else
                {
                    AConsoleManager.Instance.EnterCommand(commandIfFalse);
                }
            }
        }

        [Command("ifelseif", "Invokes a command based on the expression.")]
        private void IfElseIf(string expr1, string commandExpr1, string expr2, string commandExpr2)
        {
            Setup();
            OPCode ret = eval.Parse(expr1);
            if (ret != null && ret.CanReturn(EvalType.Boolean))
            {
                bool r = (bool)ret.Value;
                OPCode ret2 = eval.Parse(expr2);
                if (ret2 != null && ret2.CanReturn(EvalType.Boolean))
                {
                    bool r2 = (bool)ret2.Value;
                    if (r)
                    {
                        AConsoleManager.Instance.EnterCommand(commandExpr1);
                    }
                    else if (r2)
                    {
                        AConsoleManager.Instance.EnterCommand(commandExpr2);
                    }
                }
            }
        }
        [Command("ifelseif", "Invokes a command based on the expressions.")]
        private void IfElseIf(string expr1, string commandExpr1, string expr2, string commandExpr2, string elseCommand)
        {
            Setup();
            OPCode ret = eval.Parse(expr1);
            if (ret != null && ret.CanReturn(EvalType.Boolean))
            {
                bool r = (bool)ret.Value;
                OPCode ret2 = eval.Parse(expr2);
                if (ret2 != null && ret2.CanReturn(EvalType.Boolean))
                {
                    bool r2 = (bool)ret2.Value;
                    if (r)
                    {
                        AConsoleManager.Instance.EnterCommand(commandExpr1);
                    }
                    else if (r2)
                    {
                        AConsoleManager.Instance.EnterCommand(commandExpr2);
                    }
                    else
                    {
                        AConsoleManager.Instance.EnterCommand(elseCommand);
                    }
                }
            }
        }

        /// <summary>
        /// Remove all Variables and Functions of the selected type.
        /// </summary>
        /// <param name="qualifiedName">Assembly Qualified Name of the Type</param>
        [Command("eval-rem-var", "Removes a variable/function container from the evaluator")]
        public void RemoveVariable(string qualifiedName)
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
        [Command("eval-clear-var", "Clears all Variable/Function Container from the Evaluator")]
        public void ClearVariables()
        {
            eval?.mEnvironmentFunctionsList.Clear();
        }
    }
}