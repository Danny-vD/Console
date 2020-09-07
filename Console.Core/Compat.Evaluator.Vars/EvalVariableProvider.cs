using System;

using Console.Evaluator;
using Console.Evaluator.Core.OPCodes;
using Console.Vars;

namespace Compat.Evaluator.Vars
{
    /// <summary>
    /// VariableContainer Implementation with the name "eval" to be used in the EnvironmentVariable Extension.
    /// Evaluates the Expression between the Brackets $eval(MYEXPRESSION)
    /// </summary>
    public class EvalVariableProvider : VariableContainer
    {

        


        /// <summary>
        /// Public Constructor.
        /// </summary>
        public EvalVariableProvider() : base("eval")
        {
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
            

            OPCode ret;
            try
            {
                ret = Eval.Parse(parameter);
            }
            catch (Exception)
            {
                if (Eval.RaiseExceptions)
                {
                    throw;
                }

                return "EVAL_ERROR";
            }

            return ret.Value.ToString();
        }

        
    }
}