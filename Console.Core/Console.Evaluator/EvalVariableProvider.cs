using System;
using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.PropertySystem;
using Console.EnvironmentVariables;
using Console.Evaluator.Core.Enums;
using Console.Evaluator.Core.OPCodes;

namespace Console.Evaluator
{
    public class EvalVariableProvider : VariableContainer
    {
        [Property("evaluator.syntax")]
        private static ParserSyntax Syntax = ParserSyntax.CSharp;
        [Property("evaluator.casesensitive")]
        private static bool CaseSensitive;
        [Property("evaluator.exceptions.raise")]
        private static bool RaiseExceptions = true;


        private Core.Evaluator eval;
        public EvalVariableProvider() : base("eval")
        {
            CommandAttributeUtils.AddCommands(this);
            EnvironmentVariableManager.AddProvider(this);
        }

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

        private void Setup()
        {
            if (eval == null)
            {
                eval = new Core.Evaluator(Syntax, CaseSensitive);
                eval.RaiseVariableNotFoundException = RaiseExceptions;
            }
        }

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



        [Command("eval-clear-var", "Clears all Variable/Function Container from the Evaluator")]
        public void ClearVariables()
        {
            if (eval == null) return;
            eval.mEnvironmentFunctionsList.Clear();
        }
    }
}