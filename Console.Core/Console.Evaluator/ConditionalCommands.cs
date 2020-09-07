using Console.Core;
using Console.Core.CommandSystem.Attributes;
using Console.Core.ILOptimizations;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.OPCodes;

namespace Console.Evaluator
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConditionalCommands
    {

        /// <summary>
        /// Runs the command until the expression evaluates to false
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="command"></param>
        [Command(
            "while",
            HelpMessage = "Runs the Specified command until the expression evaluates to true.",
            Namespace = Eval.EVAL_NAMESPACE
        )]
        [OptimizeIL]
        public static void While(string expr, string command)
        {
            while (true)
            {
                OPCode ret = Eval.Parse(AConsoleManager.ExpanderManager.Expand(expr));
                if (ret != null && ret.CanReturn(EvalType.Boolean))
                {
                    bool r = (bool)ret.Value;
                    if (r)
                    {
                        AConsoleManager.Instance.EnterCommand(command);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        [Command(
            "if",
            HelpMessage = "If the Expression is evaluated to true the command is executed.",
            Namespace = Eval.EVAL_NAMESPACE
        )]
        private static void ConditionalIf(string expr, string command)
        {
            OPCode ret = Eval.Parse(expr);
            if (ret != null && ret.CanReturn(EvalType.Boolean))
            {
                bool r = (bool)ret.Value;
                if (r)
                {
                    AConsoleManager.Instance.EnterCommand(command);
                }
            }
        }

        [Command("ifelse", HelpMessage = "Invokes a command based on the expression.", Namespace = Eval.EVAL_NAMESPACE)]
        private static void IfElse(string expr, string commandIfTrue, string commandIfFalse)
        {
            OPCode ret = Eval.Parse(expr);
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

        [Command("ifelseif", HelpMessage = "Invokes a command based on the expression.", Namespace = Eval.EVAL_NAMESPACE)]
        private static void IfElseIf(string expr1, string commandExpr1, string expr2, string commandExpr2)
        {
            OPCode ret = Eval.Parse(expr1);
            if (ret != null && ret.CanReturn(EvalType.Boolean))
            {
                bool r = (bool)ret.Value;
                OPCode ret2 = Eval.Parse(expr2);
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

        [Command("ifelseif", HelpMessage = "Invokes a command based on the expressions.", Namespace = Eval.EVAL_NAMESPACE)]
        private static void IfElseIf(string expr1, string commandExpr1, string expr2, string commandExpr2, string elseCommand)
        {
            OPCode ret = Eval.Parse(expr1);
            if (ret != null && ret.CanReturn(EvalType.Boolean))
            {
                bool r = (bool)ret.Value;
                OPCode ret2 = Eval.Parse(expr2);
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

    }
}